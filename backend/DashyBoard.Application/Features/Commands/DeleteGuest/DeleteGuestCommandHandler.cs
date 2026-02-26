using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Application.Common.Models;
using DashyBoard.Domain.Entities;
using MediatR;

namespace DashyBoard.Application.Features.Commands.DeleteGuest;

public class DeleteGuestCommandHandler(IRepository<Guest> repository)
    : IRequestHandler<DeleteGuestCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(DeleteGuestCommand request, CancellationToken cancellationToken)
    {
        var guest = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (guest == null)
        {
            return Result<Guid>.Failure("Guest not found.");
        }

        await repository.DeleteAsync(guest, cancellationToken);

        return Result<Guid>.Success(guest.Id);
    }
}