using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Application.DTOs;
using DashyBoard.Domain.Entities;

using MediatR;

namespace DashyBoard.Application.Features.Queries.GetGuest;

public class GetGuestQueryHandler(IRepository<Guest> repository) : IRequestHandler<GetGuestQuery, GuestDto>
{
    // Inject your repository or DbContext here

    public async Task<GuestDto> Handle(GetGuestQuery request, CancellationToken cancellationToken)
    {
        var guest = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (guest == null)
        {
            return null!;
        }
        else
        {
            return new GuestDto
            {
                Id = guest.Id,
                FirstName = guest.FirstName,
                LastName = guest.LastName,
            };

        }
    }
}
