using DashyBoard.Application.Common.Models;
using DashyBoard.Application.DTOs;
using MediatR;

namespace DashyBoard.Application.Features.Commands.Rooms.UpdateRoom;

public record UpdateRoomCommand(Guid Id, Guid HotelId, string RoomNumber)
    : IRequest<Result<RoomDto>>;
