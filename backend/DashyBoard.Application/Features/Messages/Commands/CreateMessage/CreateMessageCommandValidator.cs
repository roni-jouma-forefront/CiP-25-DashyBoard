using DashyBoard.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DashyBoard.Application.Features.Messages.Commands.CreateMessage;

public class CreateMessageCommandValidator : AbstractValidator<CreateMessageCommand>
{
    private static readonly string[] ValidRecurrenceTypes = ["None", "Daily", "Weekly", "Monthly"];
    private static readonly string[] ValidDayAbbreviations =
    [
        "Mon",
        "Tue",
        "Wed",
        "Thu",
        "Fri",
        "Sat",
        "Sun",
    ];

    private readonly IApplicationDbContext _context;

    public CreateMessageCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage("Content krävs")
            .MaximumLength(500)
            .WithMessage("Content får max vara 500 tecken");

        RuleFor(x => x.PostedBy)
            .NotEmpty()
            .WithMessage("PostedBy krävs")
            .MaximumLength(100)
            .WithMessage("PostedBy får max vara 100 tecken");

        RuleFor(x => x.ExpiresAt)
            .NotEmpty()
            .WithMessage("ExpiresAt krävs")
            .GreaterThan(DateTime.UtcNow)
            .WithMessage("ExpiresAt måste vara i framtiden");

        RuleFor(x => x.RecurrenceType)
            .Must(t => ValidRecurrenceTypes.Contains(t))
            .WithMessage("RecurrenceType måste vara None, Daily, Weekly eller Monthly");

        RuleFor(x => x.RecurrenceDays)
            .NotEmpty()
            .WithMessage("RecurrenceDays krävs när RecurrenceType är Weekly")
            .Must(BeValidDays)
            .WithMessage(
                "RecurrenceDays måste innehålla giltiga dagförkortningar (Mon, Tue, Wed, Thu, Fri, Sat, Sun)"
            )
            .When(x => x.RecurrenceType == "Weekly");

        RuleFor(x => x.RecurrenceTimeEnd)
            .NotNull()
            .WithMessage("RecurrenceTimeEnd krävs när RecurrenceTimeStart är satt")
            .GreaterThan(x => x.RecurrenceTimeStart)
            .WithMessage("RecurrenceTimeEnd måste vara efter RecurrenceTimeStart")
            .When(x => x.RecurrenceTimeStart.HasValue);

        RuleFor(x => x.HotelId)
            .MustAsync(HotelExists)
            .When(x => x.HotelId.HasValue)
            .WithMessage("Hotel med angivet ID finns inte");

        RuleFor(x => x.BookingId)
            .MustAsync(BookingExists)
            .When(x => x.BookingId.HasValue)
            .WithMessage("Booking med angivet ID finns inte");
    }

    private static bool BeValidDays(string? days)
    {
        if (string.IsNullOrWhiteSpace(days))
            return false;

        return days.Split(',').Select(d => d.Trim()).All(d => ValidDayAbbreviations.Contains(d));
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
