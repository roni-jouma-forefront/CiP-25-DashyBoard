using DashyBoard.Application.DTOs;
using MediatR;

namespace DashyBoard.Application.Features.Queries.GetGuest;

public record GetGuestQuery
(
    Guid Id,
    string FirstName,
    string LastName
) : IRequest<GuestDto>;
