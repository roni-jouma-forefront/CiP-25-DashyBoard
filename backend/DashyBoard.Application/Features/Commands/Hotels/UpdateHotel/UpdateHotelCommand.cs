using DashyBoard.Application.Common.Models;
using DashyBoard.Application.DTOs;
using MediatR;

namespace DashyBoard.Application.Features.Commands.Hotels.UpdateHotel;

public record UpdateHotelCommand(Guid Id, string Name, string IcaoCode)
    : IRequest<Result<HotelDto>>;
