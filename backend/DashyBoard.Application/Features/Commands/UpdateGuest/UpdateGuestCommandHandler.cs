using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Application.Common.Models;
using DashyBoard.Application.DTOs;
using DashyBoard.Domain.Entities;
using MediatR;

namespace DashyBoard.Application.Features.Commands.UpdateGuest;

public class UpdateGuestCommandHandler(IRepository<Guest> repository, IDateTime dateTime)
    : IRequestHandler<UpdateGuestCommand, Result<GuestDto>>
{
    public async Task<Result<GuestDto>> Handle(
        UpdateGuestCommand request,
        CancellationToken cancellationToken
    )
    {
        var guest = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (guest == null)
        {
            return Result<GuestDto>.Failure("Guest not found.");
        }

        guest.FirstName = request.FirstName;
        guest.LastName = request.LastName;
        guest.UpdatedAt = dateTime.CetNow;
        guest.UpdatedBy = "work in progress";

        await repository.UpdateAsync(guest, cancellationToken);

        return Result<GuestDto>.Success(
            new GuestDto
            {
                Id = guest.Id,
                FirstName = guest.FirstName,
                LastName = guest.LastName,
            }
        );
    }
}
