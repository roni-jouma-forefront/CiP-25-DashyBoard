using DashyBoard.Application.Common.Models;
using MediatR;

namespace DashyBoard.Application.Features.Messages.Commands.DeleteMessage;

public class DeleteMessageCommand : IRequest<Result<Guid>>
{
    public Guid Id { get; set; }
}
