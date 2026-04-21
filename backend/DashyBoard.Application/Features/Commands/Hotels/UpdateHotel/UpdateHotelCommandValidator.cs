using FluentValidation;

namespace DashyBoard.Application.Features.Commands.Hotels.UpdateHotel;

public class UpdateHotelCommandValidator : AbstractValidator<UpdateHotelCommand>
{
    public UpdateHotelCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Hotel name is required")
            .MaximumLength(50)
            .WithMessage("Hotel name must not exceed 50 characters");

        RuleFor(x => x.IcaoCode)
            .NotEmpty()
            .WithMessage("ICAO code is required")
            .Length(4)
            .WithMessage("ICAO code must be exactly 4 characters");
    }
}
