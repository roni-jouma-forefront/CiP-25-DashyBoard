using DashyBoard.Application.DTOs;
using MediatR;

namespace DashyBoard.Application.Features.Queries.Rooms.GetAllRooms;

public record GetAllRoomsQuery(Guid HotelId) : IRequest<List<RoomDto>>;
