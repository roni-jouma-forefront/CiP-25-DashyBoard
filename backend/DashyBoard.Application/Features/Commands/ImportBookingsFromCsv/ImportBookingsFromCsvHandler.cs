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
                var (booking, wasUpdated) = await UpsertBookingFromRow(row, cancellationToken);
                if (wasUpdated)
                {
                    result.UpdatedBookingIds.Add(booking.Id);
                    result.UpdatedImports++;
                }
                else
                {
                    result.CreatedBookingIds.Add(booking.Id);
                }
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

    private async Task<(Booking booking, bool wasUpdated)> UpsertBookingFromRow(BookingCsvRowDto row, CancellationToken ct)
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
                IsPilot = row.IsPilot,
                CreatedAt = dateTime.UtcNow,
                CreatedBy = "csv-import",
            };
            await guestRepository.AddAsync(guest, ct);
        }

        // 2. Find room by number
        var rooms = await roomRepository.FindAsync(r => r.RoomNumber == row.RoomNumber, ct);
        var room =
            rooms.FirstOrDefault()
            ?? throw new InvalidOperationException($"Room {row.RoomNumber} not found.");

        // 3. Parse status
        if (!Enum.TryParse<Booking.Status>(row.BookingStatus, true, out var status))
            status = Booking.Status.Confirmed;

        // 4. Check if booking already exists (upsert)
        if (row.BookingId.HasValue)
        {
            var existing = await bookingRepository.GetByIdAsync(row.BookingId.Value, ct);
            if (existing != null)
            {
                existing.GuestId = guest.Id;
                existing.RoomId = room.Id;
                existing.FlightNumber = row.FlightNumber;
                existing.NumberOfGuests = row.NumberOfGuests;
                existing.CheckIn = row.CheckIn;
                existing.CheckOut = row.CheckOut;
                existing.BookingStatus = status;
                existing.UpdatedAt = dateTime.UtcNow;
                existing.UpdatedBy = "csv-import";
                await bookingRepository.UpdateAsync(existing, ct);
                return (existing, true);
            }
        }

        // 5. Create new booking
        var booking = new Booking
        {
            Id = row.BookingId ?? Guid.NewGuid(),
            GuestId = guest.Id,
            RoomId = room.Id,
            FlightNumber = row.FlightNumber,
            NumberOfGuests = row.NumberOfGuests,
            CheckIn = row.CheckIn,
            CheckOut = row.CheckOut,
            BookingStatus = status,
            CreatedAt = dateTime.UtcNow,
            CreatedBy = "csv-import",
        };

        await bookingRepository.AddAsync(booking, ct);
        return (booking, false);
    }
}
