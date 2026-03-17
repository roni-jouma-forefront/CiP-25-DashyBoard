using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DashyBoard.Application.Features.Messages.Queries.GetMessagesForMirror;

public class GetMessagesForMirrorQueryHandler
    : IRequestHandler<GetMessagesForMirrorQuery, List<MessageDto>>
{
    private readonly IApplicationDbContext _context;

    public GetMessagesForMirrorQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<MessageDto>> Handle(
        GetMessagesForMirrorQuery request,
        CancellationToken cancellationToken
    )
    {
        var now = DateTime.UtcNow;

        var query = _context
            .Messages.Where(m => m.IsActive)
            .Where(m => m.ExpiresAt > now)
            .AsQueryable();

        // Filtrera baserat på scenarion
        if (request.HotelId.HasValue && request.BookingId.HasValue)
        {
            // Båda finns - hämta allmänna meddelanden för hotellet OCH personliga för bokningen
            query = query.Where(m =>
                (m.HotelId == request.HotelId && m.BookingId == null)
                || m.BookingId == request.BookingId
            );
        }
        else if (request.HotelId.HasValue)
        {
            // Endast hotell - hämta allmänna meddelanden
            query = query.Where(m => m.HotelId == request.HotelId && m.BookingId == null);
        }
        else if (request.BookingId.HasValue)
        {
            // Endast bokning - hämta personliga meddelanden
            query = query.Where(m => m.BookingId == request.BookingId);
        }

        var messages = await query
            .OrderByDescending(m => m.CreatedAt)
            .Select(m => new MessageDto
            {
                Id = m.Id,
                HotelId = m.HotelId,
                BookingId = m.BookingId,
                Content = m.Content ?? string.Empty,
                ExpiresAt = m.ExpiresAt,
                IsActive = m.IsActive,
                CreatedAt = m.CreatedAt,
            })
            .ToListAsync(cancellationToken);

        return messages;
    }
}
