using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Domain.Entities;
using FluentValidation;

namespace DashyBoard.Application.Features.Commands.Hotels.CreateHotel;

public class CreateHotelCommandValidator : AbstractValidator<CreateHotelCommand>
{
    public CreateHotelCommandValidator(IRepository<Hotel> hotelRepository)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Hotel name is required")
            .MaximumLength(50)
            .WithMessage("Hotel name must not exceed 50 characters")
            .MustAsync(async (name, cancellation) =>
            {
                return !await hotelRepository
                    .AnyAsync(h => h.Name == name, cancellation);
            })
            .WithMessage("Hotel with the same name already exists");

        RuleFor(x => x.IcaoCode)
            .NotEmpty()
            .WithMessage("Icao Code is required")
            .Length(4)
            .WithMessage("Icao Code must be 4 characters");
    }
}
