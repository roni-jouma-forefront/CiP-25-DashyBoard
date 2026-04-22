using DashyBoard.Application.Common.Models;
using MediatR;

namespace DashyBoard.Application.Features.Commands.Hotels.DeleteHotel;

public record DeleteHotelCommand(Guid Id) : IRequest<Result<Guid>>;
