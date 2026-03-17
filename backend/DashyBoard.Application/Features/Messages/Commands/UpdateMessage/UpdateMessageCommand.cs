using DashyBoard.Application.Common.Models;
using MediatR;

namespace DashyBoard.Application.Features.Messages.Commands.UpdateMessage;

public class UpdateMessageCommand : IRequest<Result<Guid>>
{
    public Guid Id { get; set; }
    public string? Content { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public bool? IsActive { get; set; }
}
