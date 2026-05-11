using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Application.DTOs;
using DashyBoard.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static DashyBoard.Domain.Entities.Booking;

namespace DashyBoard.Application.Features.Messages.Queries.GetMessagesForRoom;

public class GetMessagesForRoomQueryHandler
    : IRequestHandler<GetMessagesForRoomQuery, List<MessageDto>>
{
    private readonly IApplicationDbContext _context;

    public GetMessagesForRoomQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<MessageDto>> Handle(
        GetMessagesForRoomQuery request,
        CancellationToken cancellationToken
    )
    {
        var utcNow = DateTime.UtcNow;
        var swedenTimeZone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
        var swedenNow = TimeZoneInfo.ConvertTimeFromUtc(utcNow, swedenTimeZone);

        var activeBooking = await _context
            .Bookings.Where(b =>
                b.RoomId == request.RoomId
                && b.BookingStatus == Status.Active
                && b.CheckIn <= utcNow
                && b.CheckOut >= utcNow
            )
            .FirstOrDefaultAsync(cancellationToken);

        var messages = await _context
            .Messages.Where(m =>
                m.HotelId == request.HotelId
                && (
                    m.BookingId == null
                    || (activeBooking != null && m.BookingId == activeBooking.Id)
                )
            )
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync(cancellationToken);

        return messages
            .Select(m => new MessageDto
            {
                Id = m.Id,
                HotelId = m.HotelId,
                BookingId = m.BookingId,
                PostedBy = m.PostedBy,
                Title = m.Title,
                Content = m.Content ?? string.Empty,
                PostAt = m.PostAt,
                ExpiresAt = m.ExpiresAt,
                IsActive = IsCurrentlyActive(m, swedenNow),
                RecurrenceType = m.RecurrenceType,
                RecurrenceDays = m.RecurrenceDays,
                RecurrenceTimeStart = m.RecurrenceTimeStart,
                RecurrenceTimeEnd = m.RecurrenceTimeEnd,
                CreatedAt = m.CreatedAt,
            })
            .ToList();
    }

    private static bool IsCurrentlyActive(Message m, DateTime swedenNow)
    {
        if (m.PostAt > swedenNow || m.ExpiresAt <= swedenNow)
            return false;

        if (m.RecurrenceType == "None")
            return true;

        if (m.RecurrenceTimeStart.HasValue && m.RecurrenceTimeEnd.HasValue)
        {
            var currentTime = TimeOnly.FromDateTime(swedenNow);
            if (
                currentTime < m.RecurrenceTimeStart.Value
                || currentTime > m.RecurrenceTimeEnd.Value
            )
                return false;
        }

        return m.RecurrenceType switch
        {
            "Daily" => true,
            "Weekly" => IsCurrentDayInRecurrenceDays(m.RecurrenceDays, swedenNow.DayOfWeek),
            "Monthly" => swedenNow.Day == m.PostAt.Day,
            _ => true,
        };
    }

    private static bool IsCurrentDayInRecurrenceDays(string? recurrenceDays, DayOfWeek dayOfWeek)
    {
        if (string.IsNullOrEmpty(recurrenceDays))
            return false;

        var dayAbbr = dayOfWeek.ToString()[..3];
        return recurrenceDays
            .Split(',')
            .Any(d => d.Trim().Equals(dayAbbr, StringComparison.OrdinalIgnoreCase));
    }
}
