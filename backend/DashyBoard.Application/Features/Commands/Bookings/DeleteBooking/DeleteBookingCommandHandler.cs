using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Application.Common.Models;
using DashyBoard.Domain.Entities;
using MediatR;

namespace DashyBoard.Application.Features.Commands.Bookings.DeleteBooking;

public class DeleteBookingCommandHandler(IRepository<Booking> repository)
    : IRequestHandler<DeleteBookingCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(
        DeleteBookingCommand request,
        CancellationToken cancellationToken
    )
    {
        var booking = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (booking == null)
        {
            return Result<Guid>.Failure("Booking not found.");
        }

        await repository.DeleteAsync(booking, cancellationToken);

        return Result<Guid>.Success(booking.Id);
    }
}
