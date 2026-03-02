using DashyBoard.Application.Common.Models;
using MediatR;

namespace DashyBoard.Application.Features.Commands.CreateGuest;

public record CreateGuestCommand(string FirstName, string LastName) : IRequest<Result<Guid>>;
