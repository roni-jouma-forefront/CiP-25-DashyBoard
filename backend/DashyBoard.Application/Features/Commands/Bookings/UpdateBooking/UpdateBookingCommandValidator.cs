using FluentValidation;

namespace DashyBoard.Application.Features.Commands.Bookings.UpdateBooking;

public class UpdateBookingCommandValidator : AbstractValidator<UpdateBookingCommand>
{
    public UpdateBookingCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Booking ID is required");

        RuleFor(x => x.NumberOfGuests)
            .GreaterThan(0)
            .WithMessage("Number of guests must be greater than zero");

        RuleFor(x => x.CheckIn)
            .NotEmpty()
            .WithMessage("Check-in date is required");

        RuleFor(x => x.CheckOut)
            .NotEmpty()
            .WithMessage("Check-out date is required")
            .GreaterThan(x => x.CheckIn)
            .WithMessage("Check-out date must be after check-in date");
    }
}
