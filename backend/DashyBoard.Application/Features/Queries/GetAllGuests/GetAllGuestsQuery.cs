using DashyBoard.Application.DTOs;
using MediatR;

namespace DashyBoard.Application.Features.Queries.GetAllGuests;

public record GetAllGuestsQuery : IRequest<List<GuestDto>>;
