using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Application.DTOs;
using DashyBoard.Domain.Entities;
using MediatR;

namespace DashyBoard.Application.Features.Queries.GetAllRooms;

public class GetAllRoomsQueryHandler(IRepository<Room> repository)
    : IRequestHandler<GetAllRoomsQuery, List<RoomDto>>
{
    public async Task<List<RoomDto>> Handle(
        GetAllRoomsQuery request,
        CancellationToken cancellationToken
    )
    {
        var rooms = await repository.FindAsync(
            r => r.HotelId == request.HotelId,
            cancellationToken
        );

        return rooms
            .Select(r => new RoomDto
            {
                Id = r.Id,
                HotelId = r.HotelId,
                RoomNumber = r.RoomNumber,
            })
            .ToList();
    }
}
