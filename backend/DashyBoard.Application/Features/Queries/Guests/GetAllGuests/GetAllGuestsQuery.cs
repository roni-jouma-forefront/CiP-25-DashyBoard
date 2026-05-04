using DashyBoard.Application.DTOs;
using MediatR;

namespace DashyBoard.Application.Features.Queries.Guests.GetAllGuests;

public record GetAllGuestsQuery(
    string? FirstName = null,
    string? LastName = null,
    bool? IsPilot = null
) : IRequest<List<GuestDto>>;
