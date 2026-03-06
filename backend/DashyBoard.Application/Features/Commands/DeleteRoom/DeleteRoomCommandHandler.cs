using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Application.Common.Models;
using DashyBoard.Domain.Entities;
using MediatR;

namespace DashyBoard.Application.Features.Commands.DeleteRoom;

public class DeleteRoomCommandHandler(IRepository<Room> repository)
    : IRequestHandler<DeleteRoomCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(
        DeleteRoomCommand request,
        CancellationToken cancellationToken
    )
    {
        var room = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (room == null)
        {
            return Result<Guid>.Failure("Room not found.");
        }

        await repository.DeleteAsync(room, cancellationToken);

        return Result<Guid>.Success(room.Id);
    }
}
