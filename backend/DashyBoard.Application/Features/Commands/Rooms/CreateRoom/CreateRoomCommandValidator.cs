using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Domain.Entities;
using FluentValidation;

namespace DashyBoard.Application.Features.Commands.CreateRoom;

public class CreateRoomCommandValidator : AbstractValidator<CreateRoomCommand>
{
    public CreateRoomCommandValidator(IRepository<Hotel> hotelRepository)
    {
        RuleFor(x => x.HotelId)
            .NotEmpty()
            .WithMessage("Hotel ID is required")
            .MustAsync(async (id, ct) => await hotelRepository.GetByIdAsync(id, ct) is not null)
            .WithMessage("Hotel does not exist");

        RuleFor(x => x.RoomNumber)
            .NotEmpty()
            .WithMessage("Room number is required")
            .MaximumLength(10)
            .WithMessage("Room number must not exceed 10 characters");
    }
}
