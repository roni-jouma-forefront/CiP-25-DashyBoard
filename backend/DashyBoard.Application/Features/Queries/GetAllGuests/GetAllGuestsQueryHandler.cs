using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Application.DTOs;
using DashyBoard.Domain.Entities;
using MediatR;

namespace DashyBoard.Application.Features.Queries.GetAllGuests;

public class GetAllGuestsQueryHandler(IRepository<Guest> repository)
    : IRequestHandler<GetAllGuestsQuery, List<GuestDto>>
{
    public async Task<List<GuestDto>> Handle(
        GetAllGuestsQuery request,
        CancellationToken cancellationToken
    )
    {
        var firstName = request.FirstName?.Trim();
        var lastName = request.LastName?.Trim();
        var firstNameLower = firstName?.ToLower();
        var lastNameLower = lastName?.ToLower();

        var hasFirstNameFilter = !string.IsNullOrWhiteSpace(firstName);
        var hasLastNameFilter = !string.IsNullOrWhiteSpace(lastName);

        var guests =
            hasFirstNameFilter || hasLastNameFilter
                ? await repository.FindAsync(
                    g =>
                        (!hasFirstNameFilter || g.FirstName.ToLower().Contains(firstNameLower!))
                        && (!hasLastNameFilter || g.LastName.ToLower().Contains(lastNameLower!)),
                    cancellationToken
                )
                : await repository.GetAllAsync(cancellationToken);

        return guests
            .Select(g => new GuestDto
            {
                Id = g.Id,
                FirstName = g.FirstName,
                LastName = g.LastName,
            })
            .ToList();
    }
}
