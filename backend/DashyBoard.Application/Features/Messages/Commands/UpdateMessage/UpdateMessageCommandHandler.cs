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

        if (request.PostedBy is not null)
            message.PostedBy = request.PostedBy;

        if (request.Title is not null)
            message.Title = request.Title;

        if (request.Content is not null)
            message.Content = request.Content;

        if (request.PostAt.HasValue)
            message.PostAt = request.PostAt.Value;

        if (request.ExpiresAt.HasValue)
            message.ExpiresAt = request.ExpiresAt.Value;

        if (request.IsActive.HasValue)
            message.IsActive = request.IsActive.Value;

        if (request.RecurrenceType is not null)
            message.RecurrenceType = request.RecurrenceType;

        if (request.RecurrenceDays is not null)
            message.RecurrenceDays = request.RecurrenceDays;

        if (request.RecurrenceTimeStart.HasValue)
            message.RecurrenceTimeStart = request.RecurrenceTimeStart;

        if (request.RecurrenceTimeEnd.HasValue)
            message.RecurrenceTimeEnd = request.RecurrenceTimeEnd;

        await repository.UpdateAsync(message, cancellationToken);

        return Result<Guid>.Success(message.Id);
    }
}
