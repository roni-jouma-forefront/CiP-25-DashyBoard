using DashyBoard.Application.Common.Models;
using DashyBoard.Application.DTOs;
using DashyBoard.Domain.Entities;
using MediatR;

namespace DashyBoard.Application.Features.Commands.Bookings.UpdateBooking;

public record UpdateBookingCommand(
    Guid Id,
    Guid? RoomId,
    Guid? GuestId,
    string FlightNumber,
    int NumberOfGuests,
    DateTime CheckIn,
    DateTime CheckOut,
    Booking.Status BookingStatus
) : IRequest<Result<BookingDto>>;
