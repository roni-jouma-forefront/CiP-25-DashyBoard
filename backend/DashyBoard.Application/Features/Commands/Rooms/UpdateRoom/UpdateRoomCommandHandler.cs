using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Application.Common.Models;
using DashyBoard.Application.DTOs;
using DashyBoard.Domain.Entities;
using MediatR;

namespace DashyBoard.Application.Features.Commands.UpdateRoom;

public class UpdateRoomCommandHandler(IRepository<Room> repository, IDateTime dateTime)
    : IRequestHandler<UpdateRoomCommand, Result<RoomDto>>
{
    public async Task<Result<RoomDto>> Handle(
        UpdateRoomCommand request,
        CancellationToken cancellationToken
    )
    {
        var room = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (room == null)
        {
            return Result<RoomDto>.Failure("Room not found.");
        }

        room.HotelId = request.HotelId;
        room.RoomNumber = request.RoomNumber;
        room.UpdatedAt = dateTime.CetNow;
        room.UpdatedBy = "work in progress";

        await repository.UpdateAsync(room, cancellationToken);

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
