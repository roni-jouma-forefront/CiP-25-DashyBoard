using DashyBoard.Application.Common.Models;
using DashyBoard.Domain.Entities;
using MediatR;

namespace DashyBoard.Application.Features.Commands.DeleteRoom;

public record DeleteRoomCommand(Guid Id, Guid HotelId) : IRequest<Result<Guid>>;
