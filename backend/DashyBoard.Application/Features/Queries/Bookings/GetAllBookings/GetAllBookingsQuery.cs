using DashyBoard.Application.DTOs;
using DashyBoard.Domain.Entities;
using MediatR;

namespace DashyBoard.Application.Features.Queries.Bookings.GetAllBookings;

public record GetAllBookingsQuery(
    Guid? GuestId = null,
    Guid? RoomId = null,
    Booking.Status? BookingStatus = null
) : IRequest<List<BookingDto>>;
