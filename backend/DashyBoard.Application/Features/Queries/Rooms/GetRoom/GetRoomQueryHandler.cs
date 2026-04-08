using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Application.DTOs;
using DashyBoard.Domain.Entities;
using MediatR;

namespace DashyBoard.Application.Features.Queries.GetRoom;

public class GetRoomQueryHandler(IRepository<Room> repository)
    : IRequestHandler<GetRoomQuery, RoomDto?>
{
    public async Task<RoomDto?> Handle(GetRoomQuery request, CancellationToken cancellationToken)
    {
        var room = (
            await repository.FindAsync(
                r => r.HotelId == request.HotelId && r.Id == request.RoomId,
                cancellationToken
            )
        ).FirstOrDefault();

        if (room == null)
        {
            return null;
        }

        return new RoomDto
        {
            Id = room.Id,
            HotelId = room.HotelId,
            RoomNumber = room.RoomNumber,
        };
    }
}
