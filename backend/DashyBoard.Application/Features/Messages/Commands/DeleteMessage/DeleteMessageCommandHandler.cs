using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Application.Common.Models;
using DashyBoard.Domain.Entities;
using MediatR;

namespace DashyBoard.Application.Features.Messages.Commands.DeleteMessage;

public class DeleteMessageCommandHandler(IRepository<Message> repository)
    : IRequestHandler<DeleteMessageCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(
        DeleteMessageCommand request,
        CancellationToken cancellationToken
    )
    {
        var message = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (message == null)
        {
            return Result<Guid>.Failure("Meddelande ej hittad");
        }

        await repository.DeleteAsync(message, cancellationToken);

        return Result<Guid>.Success(message.Id);
    }
}
