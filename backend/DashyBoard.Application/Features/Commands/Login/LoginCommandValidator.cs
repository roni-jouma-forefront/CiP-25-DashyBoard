using FluentValidation;

namespace DashyBoard.Application.Features.Commands.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.HotelId).NotEmpty().WithMessage("HotelId is required");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required")
            .MinimumLength(4)
            .WithMessage("Password must be at least 4 characters");
    }
}
