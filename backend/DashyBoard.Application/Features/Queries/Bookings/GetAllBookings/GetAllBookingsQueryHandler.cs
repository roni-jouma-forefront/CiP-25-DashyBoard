using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Application.DTOs;
using DashyBoard.Domain.Entities;
using MediatR;

namespace DashyBoard.Application.Features.Queries.Bookings.GetAllBookings;

public class GetAllBookingsQueryHandler(
    IRepository<Booking> repository,
    IRepository<Room> roomRepository,
    IRepository<Guest> guestRepository
) : IRequestHandler<GetAllBookingsQuery, List<BookingDto>>
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

        var rooms = await roomRepository.GetAllAsync(cancellationToken);
        var guests = await guestRepository.GetAllAsync(cancellationToken);

        var roomLookup = rooms.ToDictionary(r => r.Id, r => r.RoomNumber);
        var guestLookup = guests.ToDictionary(g => g.Id, g => $"{g.FirstName} {g.LastName}");

        return bookings
            .Select(b => new BookingDto
            {
                Id = b.Id,
                RoomId = b.RoomId,
                GuestId = b.GuestId,
                RoomNumber = b.RoomId.HasValue && roomLookup.TryGetValue(b.RoomId.Value, out var rn) ? rn : null,
                GuestName = b.GuestId.HasValue && guestLookup.TryGetValue(b.GuestId.Value, out var gn) ? gn : null,
                FlightNumber = b.FlightNumber,
                NumberOfGuests = b.NumberOfGuests,
                CheckIn = b.CheckIn,
                CheckOut = b.CheckOut,
                BookingStatus = b.BookingStatus,
            })
            .ToList();
    }
}
