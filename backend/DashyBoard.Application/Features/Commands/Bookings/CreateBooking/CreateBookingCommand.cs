using DashyBoard.Application.Common.Models;
using DashyBoard.Application.DTOs;
using DashyBoard.Domain.Entities;
using MediatR;

namespace DashyBoard.Application.Features.Commands.Bookings.CreateBooking;

public record CreateBookingCommand(
    Guid? RoomId,
    Guid? GuestId,
    Guid? FlightId,
    int NumberOfGuests,
    DateTime CheckIn,
    DateTime CheckOut,
    Booking.Status BookingStatus
) : IRequest<Result<BookingDto>>;
