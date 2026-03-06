using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Application.Common.Models;
using DashyBoard.Application.DTOs;
using DashyBoard.Domain.Entities;
using MediatR;

namespace DashyBoard.Application.Features.Commands.CreateGuest;

public class CreateGuestCommandHandler(IRepository<Guest> repository, IDateTime dateTime)
    : IRequestHandler<CreateGuestCommand, Result<GuestDto>>
{
    public async Task<Result<GuestDto>> Handle(
        CreateGuestCommand request,
        CancellationToken cancellationToken
    )
    {
        var guest = new Guest
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            CreatedAt = dateTime.CetNow,
        };

        await repository.AddAsync(guest, cancellationToken);

        return Result<GuestDto>.Success(
            new GuestDto
            {
                Id = guest.Id,
                FirstName = guest.FirstName!,
                LastName = guest.LastName!,
            }
        );
    }
}
