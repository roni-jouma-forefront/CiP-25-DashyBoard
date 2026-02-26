using DashyBoard.Application.Common.Models;
using MediatR;

namespace DashyBoard.Application.Features.Commands.UpdateGuest;

public record UpdateGuestCommand(Guid Id, string FirstName, string LastName) : IRequest<Result<Guid>>;
