using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Application.DTOs;
using DashyBoard.Domain.Entities;
using MediatR;

namespace DashyBoard.Application.Features.Queries.Bookings.GetBookingWithGuest;

/// <summary>
/// Handler that fetches a booking for a room with guest info.
/// Used for mirror display.
/// </summary>
public class GetBookingWithGuestQueryHandler(
    IRepository<Booking> bookingRepository,
    IRepository<Guest> guestRepository
) : IRequestHandler<GetBookingWithGuestQuery, BookingWithGuestDto?>
{
    public async Task<BookingWithGuestDto?> Handle(
        GetBookingWithGuestQuery request,
        CancellationToken cancellationToken
    )
    {
        // 1. Find booking for the room (optionally filtered by status)
        var bookings = await bookingRepository.FindAsync(
            b =>
                b.RoomId == request.RoomId
                && (!request.BookingStatus.HasValue || b.BookingStatus == request.BookingStatus),
            cancellationToken
        );

        var booking = bookings.FirstOrDefault();
        if (booking == null)
            return null;

        // 2. Get guest info if available
        GuestInfoDto? guestInfo = null;
        if (booking.GuestId.HasValue)
        {
            var guest = await guestRepository.GetByIdAsync(
                booking.GuestId.Value,
                cancellationToken
            );
            if (guest != null)
            {
                guestInfo = new GuestInfoDto
                {
                    Id = guest.Id,
                    FirstName = guest.FirstName,
                    LastName = guest.LastName,
                };
            }
        }

        // 3. Build enriched DTO
        return new BookingWithGuestDto
        {
            Id = booking.Id,
            RoomId = booking.RoomId,
            FlightNumber = booking.FlightNumber,
            NumberOfGuests = booking.NumberOfGuests,
            CheckIn = booking.CheckIn,
            CheckOut = booking.CheckOut,
            BookingStatus = booking.BookingStatus,
            Guest = guestInfo,
        };
    }
}
