using DashyBoard.Application.DTOs;
using MediatR;

namespace DashyBoard.Application.Features.Queries.Bookings.GetBooking;

public record GetBookingQuery(Guid Id) : IRequest<BookingDto>;
