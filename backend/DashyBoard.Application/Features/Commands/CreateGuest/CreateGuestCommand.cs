using DashyBoard.Application.Common.Models;
using DashyBoard.Application.DTOs;
using MediatR;

namespace DashyBoard.Application.Features.Commands.CreateGuest;

public record CreateGuestCommand(string FirstName, string LastName) : IRequest<Result<GuestDto>>;
