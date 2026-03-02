using DashyBoard.Application.DTOs.Swedavia;
using MediatR;

namespace DashyBoard.Application.Features.Flights.Queries;

public record GetArrivalsQuery(string Airport, DateOnly Date, string? FlightId = null)
    : IRequest<IEnumerable<FlightInfoDto>>;
