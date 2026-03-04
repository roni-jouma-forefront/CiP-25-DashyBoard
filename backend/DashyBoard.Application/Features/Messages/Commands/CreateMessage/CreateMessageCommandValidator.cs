using DashyBoard.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DashyBoard.Application.Features.Messages.Commands.CreateMessage;

public class CreateMessageCommandValidator : AbstractValidator<CreateMessageCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateMessageCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage("Content kr�vs")
            .MaximumLength(500)
            .WithMessage("Content f�r max vara 500 tecken");

        RuleFor(x => x.ExpiresAt)
            .NotEmpty()
            .WithMessage("ExpiresAt kr�vs")
            .GreaterThan(DateTime.UtcNow)
            .WithMessage("ExpiresAt m�ste vara i framtiden");

        RuleFor(x => x.HotelId)
            .MustAsync(HotelExists)
            .When(x => x.HotelId.HasValue)
            .WithMessage("Hotel med angivet ID finns inte");

        RuleFor(x => x.BookingId)
            .MustAsync(BookingExists)
            .When(x => x.BookingId.HasValue)
            .WithMessage("Booking med angivet ID finns inte");
    }

    private async Task<bool> HotelExists(Guid? hotelId, CancellationToken cancellationToken)
    {
        if (!hotelId.HasValue)
            return true;
        return await _context.Hotels.AnyAsync(h => h.Id == hotelId.Value, cancellationToken);
    }

    private async Task<bool> BookingExists(Guid? bookingId, CancellationToken cancellationToken)
    {
        if (!bookingId.HasValue)
            return true;
        return await _context.Bookings.AnyAsync(b => b.Id == bookingId.Value, cancellationToken);
    }
}
