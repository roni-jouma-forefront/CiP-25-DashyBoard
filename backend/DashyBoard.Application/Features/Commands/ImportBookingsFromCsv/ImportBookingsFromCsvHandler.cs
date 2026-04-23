using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Application.Common.Interfaces.External;
using DashyBoard.Application.Common.Models;
using DashyBoard.Application.DTOs;
using DashyBoard.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DashyBoard.Application.Features.Commands.ImportBookingsFromCsv;

public class ImportBookingsFromCsvHandler(
    IBookingCsvParser csvParser,
    IRepository<Booking> bookingRepository,
    IRepository<Guest> guestRepository,
    IRepository<Room> roomRepository,
    IRepository<Flight> flightRepository,
    ISwedaviaFlightApiService flightApiService,
    IDateTime dateTime,
    ILogger<ImportBookingsFromCsvHandler> logger
) : IRequestHandler<ImportBookingsFromCsvCommand, Result<CsvImportResultDto>>
{
    public async Task<Result<CsvImportResultDto>> Handle(
        ImportBookingsFromCsvCommand request,
        CancellationToken cancellationToken
    )
    {
        var result = new CsvImportResultDto();

        IEnumerable<BookingCsvRowDto> rows;
        try
        {
            rows = await csvParser.ParseAsync(request.CsvStream, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to parse CSV file");
            return Result<CsvImportResultDto>.Failure($"Failed to parse CSV: {ex.Message}");
        }

        var rowList = rows.ToList();
        result.TotalRows = rowList.Count;

        foreach (var row in rowList)
        {
            try
            {
                var booking = await CreateBookingFromRow(row, cancellationToken);
                result.CreatedBookingIds.Add(booking.Id);
                result.SuccessfulImports++;
            }
            catch (Exception ex)
            {
                result.FailedImports++;
                result.Errors.Add(
                    $"Row for {row.GuestFirstName} {row.GuestLastName}: {ex.Message}"
                );
                logger.LogWarning(
                    ex,
                    "Failed to import booking for {First} {Last}",
                    row.GuestFirstName,
                    row.GuestLastName
                );
            }
        }

        return Result<CsvImportResultDto>.Success(result);
    }

    private async Task<Booking> CreateBookingFromRow(BookingCsvRowDto row, CancellationToken ct)
    {
        // 1. Find or create guest
        var guests = await guestRepository.FindAsync(
            g => g.FirstName == row.GuestFirstName && g.LastName == row.GuestLastName,
            ct
        );
        var guest = guests.FirstOrDefault();
        if (guest == null)
        {
            guest = new Guest
            {
                Id = Guid.NewGuid(),
                FirstName = row.GuestFirstName,
                LastName = row.GuestLastName,
                CreatedAt = dateTime.CetNow,
                CreatedBy = "csv-import",
            };
            await guestRepository.AddAsync(guest, ct);
        }

        // 2. Find room by number
        var rooms = await roomRepository.FindAsync(r => r.RoomNumber == row.RoomNumber, ct);
        var room =
            rooms.FirstOrDefault()
            ?? throw new InvalidOperationException($"Room {row.RoomNumber} not found.");

        // 3. Fetch flight from Swedavia API and persist
        Guid? flightId = null;
        if (!string.IsNullOrWhiteSpace(row.FlightNumber))
        {
            flightId = await ResolveFlightAsync(row, ct);
        }

        // 4. Parse status
        if (!Enum.TryParse<Booking.Status>(row.BookingStatus, true, out var status))
            status = Booking.Status.Confirmed;

        // 5. Create booking
        var booking = new Booking
        {
            Id = Guid.NewGuid(),
            GuestId = guest.Id,
            RoomId = room.Id,
            FlightId = flightId,
            NumberOfGuests = row.NumberOfGuests,
            CheckIn = row.CheckIn,
            CheckOut = row.CheckOut,
            BookingStatus = status,
            CreatedAt = dateTime.CetNow,
            CreatedBy = "csv-import",
        };

        await bookingRepository.AddAsync(booking, ct);
        return booking;
    }

    private async Task<Guid?> ResolveFlightAsync(BookingCsvRowDto row, CancellationToken ct)
    {
        try
        {
            var isArrival = string.Equals(
                row.FlightType,
                "Arrival",
                StringComparison.OrdinalIgnoreCase
            );
            var date = DateOnly.FromDateTime(isArrival ? row.CheckIn : row.CheckOut);

            var flights = isArrival
                ? await flightApiService.GetArrivalsAsync(row.FlightNumber, "ARN", date, ct)
                : await flightApiService.GetDeparturesAsync(row.FlightNumber, "ARN", date, ct);

            var flightInfo = flights.FirstOrDefault();
            if (flightInfo == null)
            {
                logger.LogWarning(
                    "Flight {FlightNumber} not found via Swedavia API",
                    row.FlightNumber
                );
                return null;
            }

            // Extract numeric part of flight number for DB lookup
            var numericPart = new string(row.FlightNumber.Where(char.IsDigit).ToArray());
            string? flightNum = string.IsNullOrWhiteSpace(numericPart) ? null : numericPart;

            // Check if flight already in DB
            var existing = await flightRepository.FindAsync(
                f =>
                    f.FlightNumber == flightNum
                    && f.Type
                        == (isArrival ? Flight.FlightType.Arrival : Flight.FlightType.Departure),
                ct
            );
            var flight = existing.FirstOrDefault();

            if (flight == null)
            {
                flight = new Flight
                {
                    Id = Guid.NewGuid(),
                    FlightNumber = flightNum,
                    Gate = flightInfo.LocationAndStatus?.Gate,
                    Status = flightInfo.LocationAndStatus?.FlightLegStatusEnglish,
                    ScheduledArrival = flightInfo.ArrivalTime?.ScheduledUtc,
                    ScheduledDeparture = flightInfo.DepartureTime?.ScheduledUtc,
                    Type = isArrival ? Flight.FlightType.Arrival : Flight.FlightType.Departure,
                    CreatedAt = dateTime.CetNow,
                    CreatedBy = "csv-import",
                };
                await flightRepository.AddAsync(flight, ct);
            }

            return flight.Id;
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Could not resolve flight {FlightNumber}", row.FlightNumber);
            return null;
        }
    }
}
