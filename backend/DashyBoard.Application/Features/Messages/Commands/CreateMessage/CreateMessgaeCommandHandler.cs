using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Application.Common.Models;
using DashyBoard.Domain.Entities;
using MediatR;

namespace DashyBoard.Application.Features.Messages.Commands.CreateMessage;

public class CreateMessageCommandHandler : IRequestHandler<CreateMessageCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;

    public CreateMessageCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<int>> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
    {
        var message = new Message
        {
            HotelId = request.HotelId,
            BookingId = request.BookingId,
            Content = request.Content,
            ExpiresAt = request.ExpiresAt ?? DateTime.UtcNow.AddDays(1),
            IsActive = true
        };

        _context.Messages.Add(message);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<int>.Success(message.Id);
    }
}