using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Application.DTOs;
using DashyBoard.Domain.Entities;
using MediatR;

namespace DashyBoard.Application.Features.Queries.GetAllGuests;


public class GetAllGuestsQueryHandler(IRepository<Guest> repository) : IRequestHandler<GetAllGuestsQuery, List<GuestDto>>
{
    public async Task<List<GuestDto>> Handle(GetAllGuestsQuery request, CancellationToken cancellationToken)
    {
        var guests = await repository.GetAllAsync(cancellationToken);

        return guests.Select(g => new GuestDto
        {
            Id = g.Id,
            FirstName = g.FirstName,
            LastName = g.LastName,
        }).ToList();
    }
}
