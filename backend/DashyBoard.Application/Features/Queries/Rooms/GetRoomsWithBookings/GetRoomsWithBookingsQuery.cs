using DashyBoard.Application.DTOs;
using MediatR;

namespace DashyBoard.Application.Features.Queries.Rooms.GetRoomsWithBookings;

/// <summary>
/// Query to get all rooms for a hotel with their active bookings and guest info.
/// Returns enriched data in a single query to avoid N+1 problem.
/// </summary>
public record GetRoomsWithBookingsQuery(Guid HotelId) : IRequest<List<RoomWithBookingDto>>;
