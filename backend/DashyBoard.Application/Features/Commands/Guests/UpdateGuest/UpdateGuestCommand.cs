using DashyBoard.Application.Common.Models;
using DashyBoard.Application.DTOs;
using MediatR;

namespace DashyBoard.Application.Features.Commands.Guests.UpdateGuest;

public record UpdateGuestCommand(Guid Id, string FirstName, string LastName)
    : IRequest<Result<GuestDto>>;
