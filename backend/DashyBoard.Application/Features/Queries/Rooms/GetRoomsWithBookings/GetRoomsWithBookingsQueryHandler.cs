using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Application.DTOs;
using DashyBoard.Domain.Entities;
using MediatR;

namespace DashyBoard.Application.Features.Queries.Rooms.GetRoomsWithBookings;

/// <summary>
/// Handler that fetches rooms with active bookings and guest info in bulk.
/// Performs 3 database queries and joins in memory to avoid N+1 problem.
/// </summary>
public class GetRoomsWithBookingsQueryHandler(
    IRepository<Room> roomRepository,
    IRepository<Booking> bookingRepository,
    IRepository<Guest> guestRepository
) : IRequestHandler<GetRoomsWithBookingsQuery, List<RoomWithBookingDto>>
{
    public async Task<List<RoomWithBookingDto>> Handle(
        GetRoomsWithBookingsQuery request,
        CancellationToken cancellationToken
    )
    {
        // 1. Get all rooms for the hotel
        var rooms = await roomRepository.FindAsync(
            r => r.HotelId == request.HotelId,
            cancellationToken
        );

        // 2. Get all active bookings (in bulk)
        var activeBookings = await bookingRepository.FindAsync(
            b => b.BookingStatus == Booking.Status.Active,
            cancellationToken
        );

        // 3. Get guest IDs from active bookings
        var guestIds = activeBookings
            .Where(b => b.GuestId.HasValue)
            .Select(b => b.GuestId!.Value)
            .Distinct()
            .ToList();

        // 4. Get all relevant guests (in bulk)
        var guests =
            guestIds.Count > 0
                ? await guestRepository.FindAsync(g => guestIds.Contains(g.Id), cancellationToken)
                : [];

        // 5. Create lookup dictionaries for efficient joining
        var guestLookup = guests.ToDictionary(g => g.Id);
        var bookingByRoomId = activeBookings
            .Where(b => b.RoomId.HasValue)
            .GroupBy(b => b.RoomId!.Value)
            .ToDictionary(g => g.Key, g => g.First()); // One active booking per room

        // 6. Build enriched DTOs
        return rooms
            .Select(room =>
            {
                BookingWithGuestDto? activeBooking = null;

                if (bookingByRoomId.TryGetValue(room.Id, out var booking))
                {
                    GuestInfoDto? guestInfo = null;

                    if (
                        booking.GuestId.HasValue
                        && guestLookup.TryGetValue(booking.GuestId.Value, out var guest)
                    )
                    {
                        guestInfo = new GuestInfoDto
                        {
                            Id = guest.Id,
                            FirstName = guest.FirstName,
                            LastName = guest.LastName,
                        };
                    }

                    activeBooking = new BookingWithGuestDto
                    {
                        Id = booking.Id,
                        RoomId = booking.RoomId,
                        FlightId = booking.FlightId,
                        NumberOfGuests = booking.NumberOfGuests,
                        CheckIn = booking.CheckIn,
                        CheckOut = booking.CheckOut,
                        BookingStatus = booking.BookingStatus,
                        Guest = guestInfo,
                    };
                }

                return new RoomWithBookingDto
                {
                    Id = room.Id,
                    HotelId = room.HotelId,
                    RoomNumber = room.RoomNumber,
                    ActiveBooking = activeBooking,
                };
            })
            .OrderBy(r => r.RoomNumber)
            .ToList();
    }
}
