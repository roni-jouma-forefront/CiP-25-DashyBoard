using DashyBoard.Application.DTOs;
using MediatR;

namespace DashyBoard.Application.Features.Queries.GetAllGuests;

public record GetAllGuestsQuery(string? FirstName = null, string? LastName = null)
    : IRequest<List<GuestDto>>;
