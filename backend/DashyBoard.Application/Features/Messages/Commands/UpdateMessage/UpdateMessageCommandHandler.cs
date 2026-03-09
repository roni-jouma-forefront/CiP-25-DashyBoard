using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Application.Common.Models;
using DashyBoard.Domain.Entities;
using MediatR;

namespace DashyBoard.Application.Features.Messages.Commands.UpdateMessage;

public class UpdateMessageCommandHandler(IRepository<Message> repository)
    : IRequestHandler<UpdateMessageCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(
        UpdateMessageCommand request,
        CancellationToken cancellationToken
    )
    {
        var message = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (message == null)
        {
            return Result<Guid>.Failure("Meddelande ej hittad");
        }

        if (string.IsNullOrWhiteSpace(request.Content))
        {
            return Result<Guid>.Failure("Innnehall kravs");
        }

        message.Content = request.Content;

        if (request.ExpiresAt.HasValue)
        {
            message.ExpiresAt = request.ExpiresAt.Value;
        }

        if (request.IsActive.HasValue)
        {
            message.IsActive = request.IsActive.Value;
        }

        await repository.UpdateAsync(message, cancellationToken);

        return Result<Guid>.Success(message.Id);
    }
}
