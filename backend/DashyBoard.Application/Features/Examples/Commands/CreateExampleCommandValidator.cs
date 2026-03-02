using FluentValidation;

namespace DashyBoard.Application.Features.Examples.Commands;

public class CreateExampleCommandValidator : AbstractValidator<CreateExampleCommand>
{
    public CreateExampleCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(200)
            .WithMessage("Name must not exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .WithMessage("Description must not exceed 1000 characters");
    }
}
