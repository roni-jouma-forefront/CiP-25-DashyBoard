using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Application.DTOs;
using DashyBoard.Domain.Entities;
using MediatR;

namespace DashyBoard.Application.Features.Queries.Bookings.GetAllBookings;

public class GetAllBookingsQueryHandler(IRepository<Booking> repository)
    : IRequestHandler<GetAllBookingsQuery, List<BookingDto>>
{
    public async Task<List<BookingDto>> Handle(
        GetAllBookingsQuery request,
        CancellationToken cancellationToken
    )
    {
        var hasGuestFilter = request.GuestId.HasValue;
        var hasRoomFilter = request.RoomId.HasValue;
        var hasStatusFilter = request.BookingStatus.HasValue;

        var bookings =
            hasGuestFilter || hasRoomFilter || hasStatusFilter
                ? await repository.FindAsync(
                    b =>
                        (!hasGuestFilter || b.GuestId == request.GuestId)
                        && (!hasRoomFilter || b.RoomId == request.RoomId)
                        && (!hasStatusFilter || b.BookingStatus == request.BookingStatus),
                    cancellationToken
                )
                : await repository.GetAllAsync(cancellationToken);

        return bookings
            .Select(b => new BookingDto
            {
                Id = b.Id,
                RoomId = b.RoomId,
                GuestId = b.GuestId,
                FlightId = b.FlightId,
                NumberOfGuests = b.NumberOfGuests,
                CheckIn = b.CheckIn,
                CheckOut = b.CheckOut,
                BookingStatus = b.BookingStatus,
            })
            .ToList();
    }
}
