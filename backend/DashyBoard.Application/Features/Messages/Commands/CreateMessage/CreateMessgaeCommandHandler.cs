using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Application.Common.Models;
using DashyBoard.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DashyBoard.Application.Features.Messages.Commands.CreateMessage;

public class CreateMessageCommandHandler : IRequestHandler<CreateMessageCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;

    public CreateMessageCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Guid>> Handle(
        CreateMessageCommand request,
        CancellationToken cancellationToken
    )
    {
        if (request.HotelId.HasValue)
        {
            var hotelExists = await _context.Hotels.AnyAsync(
                h => h.Id == request.HotelId.Value,
                cancellationToken
            );

            if (!hotelExists)
            {
                return Result<Guid>.Failure("Hotel med angivet ID finns inte");
            }
        }

        if (request.BookingId.HasValue)
        {
            var bookingExists = await _context.Bookings.AnyAsync(
                b => b.Id == request.BookingId.Value,
                cancellationToken
            );

            if (!bookingExists)
            {
                return Result<Guid>.Failure("Booking med angivet ID finns inte");
            }
        }

        var utcNow = DateTime.UtcNow;
        var swedenTimeZone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
        var swedenNow = TimeZoneInfo.ConvertTimeFromUtc(utcNow, swedenTimeZone);
        var truncatedNow = new DateTime(
            swedenNow.Year,
            swedenNow.Month,
            swedenNow.Day,
            swedenNow.Hour,
            swedenNow.Minute,
            swedenNow.Second,
            DateTimeKind.Unspecified
        );

        var message = new Message
        {
            HotelId = request.HotelId,
            BookingId = request.BookingId,
            Content = request.Content,
            ExpiresAt = request.ExpiresAt ?? truncatedNow.AddDays(1),
            IsActive = true,
        };

        _context.Messages.Add(message);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(message.Id);
    }
}
