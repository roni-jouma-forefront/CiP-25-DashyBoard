using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Application.Common.Models;
using DashyBoard.Domain.Entities;
using MediatR;

namespace DashyBoard.Application.Features.Commands.CreateGuest;

public class CreateGuestCommandHandler(IRepository<Guest> repository)
    : IRequestHandler<CreateGuestCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateGuestCommand request, CancellationToken cancellationToken)
    {
        var guest = new Guest
        {
            Id        = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName  = request.LastName,
        };

        await repository.AddAsync(guest, cancellationToken);

        return Result<Guid>.Success(guest.Id);
    }
}
