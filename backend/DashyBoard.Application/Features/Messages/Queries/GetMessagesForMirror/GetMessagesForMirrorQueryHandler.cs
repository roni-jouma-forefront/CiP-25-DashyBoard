using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DashyBoard.Application.Features.Messages.Queries.GetMessagesForMirror;

public class GetMessagesForMirrorQueryHandler : IRequestHandler<GetMessagesForMirrorQuery, List<MessageDto>>
{
    private readonly IApplicationDbContext _context;

    public GetMessagesForMirrorQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<MessageDto>> Handle(GetMessagesForMirrorQuery request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var messages = await _context.Messages
            .Where(m =>
                (m.HotelId == request.HotelId && m.BookingId == null)
                ||
                (m.BookingId == request.BookingId && m.BookingId != null)
            )
            .Where(m => m.IsActive)
            .Where(m => m.ExpiresAt > now)
            .OrderByDescending(m => m.CreatedAt)
            .Select(m => new MessageDto
            {
                Id = m.Id,
                HotelId = m.HotelId,
                BookingId = m.BookingId,
                Content = m.Content ?? string.Empty,
                ExpiresAt = m.ExpiresAt,
                IsActive = m.IsActive
            })
            .ToListAsync(cancellationToken);

        return messages;
    }
}