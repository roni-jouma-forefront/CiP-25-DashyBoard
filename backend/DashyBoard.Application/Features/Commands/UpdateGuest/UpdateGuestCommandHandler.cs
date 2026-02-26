using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Application.Common.Models;
using DashyBoard.Domain.Entities;
using MediatR;

namespace DashyBoard.Application.Features.Commands.UpdateGuest;

public class UpdateGuestCommandHandler(IRepository<Guest> repository)
    : IRequestHandler<UpdateGuestCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(UpdateGuestCommand request, CancellationToken cancellationToken)
    {
        var guest = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (guest == null)
        {
            return Result<Guid>.Failure("Guest not found.");
        }

        guest.FirstName = request.FirstName;
        guest.LastName = request.LastName;

        await repository.UpdateAsync(guest, cancellationToken);

        return Result<Guid>.Success(guest.Id);
    }
}