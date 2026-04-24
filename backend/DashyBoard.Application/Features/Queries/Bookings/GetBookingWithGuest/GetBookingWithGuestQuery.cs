using DashyBoard.Application.DTOs;
using DashyBoard.Domain.Entities;
using MediatR;

namespace DashyBoard.Application.Features.Queries.Bookings.GetBookingWithGuest;

/// <summary>
/// Query to get active booking for a room with guest info.
/// Used for mirror display to get booking + guest in one call.
/// </summary>
public record GetBookingWithGuestQuery(Guid RoomId, Booking.Status? BookingStatus = null)
    : IRequest<BookingWithGuestDto?>;
