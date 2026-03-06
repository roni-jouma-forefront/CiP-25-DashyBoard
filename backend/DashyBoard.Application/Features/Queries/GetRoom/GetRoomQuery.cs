using DashyBoard.Application.DTOs;
using MediatR;

namespace DashyBoard.Application.Features.Queries.GetRoom;

public record GetRoomQuery(Guid HotelId, Guid RoomId) : IRequest<RoomDto?>;
