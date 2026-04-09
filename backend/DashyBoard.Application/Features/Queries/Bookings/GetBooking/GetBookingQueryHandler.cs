using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Application.DTOs;
using DashyBoard.Domain.Entities;
using MediatR;

namespace DashyBoard.Application.Features.Queries.Bookings.GetBooking;

public class GetBookingQueryHandler(IRepository<Booking> repository)
    : IRequestHandler<GetBookingQuery, BookingDto>
{
    public async Task<BookingDto> Handle(
        GetBookingQuery request,
        CancellationToken cancellationToken
    )
    {
        var booking = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (booking == null)
        {
            return null!;
        }

        return new BookingDto
        {
            Id = booking.Id,
            RoomId = booking.RoomId,
            GuestId = booking.GuestId,
            FlightId = booking.FlightId,
            NumberOfGuests = booking.NumberOfGuests,
            CheckIn = booking.CheckIn,
            CheckOut = booking.CheckOut,
            BookingStatus = booking.BookingStatus,
        };
    }
}
