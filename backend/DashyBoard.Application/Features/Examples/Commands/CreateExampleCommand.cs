using DashyBoard.Application.Common.Models;
using MediatR;

namespace DashyBoard.Application.Features.Examples.Commands;

public record CreateExampleCommand(string Name, string Description) : IRequest<Result<Guid>>;
