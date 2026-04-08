using DashyBoard.Application.Common.Models;
using DashyBoard.Application.DTOs;
using MediatR;

namespace DashyBoard.Application.Features.Commands.Rooms.CreateRoom;

public record CreateRoomCommand(Guid HotelId, string RoomNumber) : IRequest<Result<RoomDto>>;
