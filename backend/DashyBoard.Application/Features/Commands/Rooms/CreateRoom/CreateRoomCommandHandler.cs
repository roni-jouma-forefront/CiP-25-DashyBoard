using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Application.Common.Models;
using DashyBoard.Application.DTOs;
using DashyBoard.Domain.Entities;
using MediatR;

namespace DashyBoard.Application.Features.Commands.CreateRoom;

public class CreateRoomCommandHandler(IRepository<Room> repository, IDateTime dateTime)
    : IRequestHandler<CreateRoomCommand, Result<RoomDto>>
{
    public async Task<Result<RoomDto>> Handle(
        CreateRoomCommand request,
        CancellationToken cancellationToken
    )
    {
        var room = new Room
        {
            Id = Guid.NewGuid(),
            HotelId = request.HotelId,
            RoomNumber = request.RoomNumber,
            CreatedAt = dateTime.CetNow,
            CreatedBy = "work in progress",
        };

        await repository.AddAsync(room, cancellationToken);

        return Result<RoomDto>.Success(
            new RoomDto
            {
                Id = room.Id,
                HotelId = room.HotelId,
                RoomNumber = room.RoomNumber,
            }
        );
    }
}
