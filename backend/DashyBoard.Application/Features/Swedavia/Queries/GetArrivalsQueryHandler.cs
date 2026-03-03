using DashyBoard.Application.Common.Interfaces.External;
using DashyBoard.Application.DTOs.Swedavia;
using MediatR;

namespace DashyBoard.Application.Features.Flights.Queries;

public class GetArrivalsQueryHandler : IRequestHandler<GetArrivalsQuery, IEnumerable<FlightInfoDto>>
{
    private readonly ISwedaviaFlightApiService _flightApiService;

    public GetArrivalsQueryHandler(ISwedaviaFlightApiService flightApiService)
    {
        _flightApiService = flightApiService;
    }

    public async Task<IEnumerable<FlightInfoDto>> Handle(
        GetArrivalsQuery request,
        CancellationToken cancellationToken
    )
    {
        return await _flightApiService.GetArrivalsAsync(
            request.FlightId ?? string.Empty,
            request.Airport,
            request.Date,
            cancellationToken
        );
    }
}
