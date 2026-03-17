using DashyBoard.Application.Common.Models;
using DashyBoard.Application.DTOs;
using MediatR;

namespace DashyBoard.Application.Features.Commands.Login;

public record LoginCommand(Guid HotelId, string Password) : IRequest<Result<LoginResponseDto>>;
