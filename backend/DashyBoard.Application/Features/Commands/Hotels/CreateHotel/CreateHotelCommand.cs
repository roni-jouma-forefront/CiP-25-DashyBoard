using DashyBoard.Application.Common.Models;
using DashyBoard.Application.DTOs;
using MediatR;

namespace DashyBoard.Application.Features.Commands.Hotels.CreateHotel;

public record CreateHotelCommand(string Name, string IcaoCode) : IRequest<Result<HotelDto>>;
