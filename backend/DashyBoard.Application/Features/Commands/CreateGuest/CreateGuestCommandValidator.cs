using FluentValidation;

namespace DashyBoard.Application.Features.Commands.CreateGuest;

public class CreateGuestCommandValidator : AbstractValidator<CreateGuestCommand>
{
    public CreateGuestCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(35).WithMessage("First name must not exceed 35 characters");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(35).WithMessage("Last name must not exceed 35 characters");
    }
}
