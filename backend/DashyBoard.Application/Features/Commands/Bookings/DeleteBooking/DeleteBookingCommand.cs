using DashyBoard.Application.Common.Models;
using MediatR;

namespace DashyBoard.Application.Features.Commands.Bookings.DeleteBooking;

public record DeleteBookingCommand(Guid Id) : IRequest<Result<Guid>>;
