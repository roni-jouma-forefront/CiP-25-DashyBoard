using DashyBoard.Application.DTOs;
using MediatR;

namespace DashyBoard.Application.Features.Queries.Guests.GetGuest;

public record GetGuestQuery(Guid Id) : IRequest<GuestDto>;
