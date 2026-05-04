using DashyBoard.Application.Common.Models;
using DashyBoard.Application.DTOs;
using MediatR;

namespace DashyBoard.Application.Features.Commands.Guests.CreateGuest;

public record CreateGuestCommand(string FirstName, string LastName, bool IsPilot)
    : IRequest<Result<GuestDto>>;
