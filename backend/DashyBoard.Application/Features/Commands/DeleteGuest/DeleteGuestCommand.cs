using DashyBoard.Application.Common.Models;
using MediatR;

namespace DashyBoard.Application.Features.Commands.DeleteGuest;

public record DeleteGuestCommand(Guid Id) : IRequest<Result<Guid>>;
