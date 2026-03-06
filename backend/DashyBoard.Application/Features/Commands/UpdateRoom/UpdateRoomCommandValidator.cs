using FluentValidation;

namespace DashyBoard.Application.Features.Commands.UpdateRoom;

public class UpdateRoomCommandValidator : AbstractValidator<UpdateRoomCommand>
{
    public UpdateRoomCommandValidator()
    {
        RuleFor(x => x.HotelId).NotEmpty().WithMessage("Hotel ID is required");

        RuleFor(x => x.RoomNumber)
            .NotEmpty()
            .WithMessage("Room number is required")
            .MaximumLength(25)
            .WithMessage("Room number must not exceed 25 characters");
    }
}
