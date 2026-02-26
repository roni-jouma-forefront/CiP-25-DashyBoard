using DashyBoard.Application.Common.Interfaces.External;
using DashyBoard.Application.DTOs.Swedavia;
using MediatR;

namespace DashyBoard.Application.Features.Flights.Queries;

public class GetDeparturesQueryHandler : IRequestHandler<GetDeparturesQuery, IEnumerable<FlightInfoDto>>
{
    private readonly ISwedaviaFlightApiService _flightApiService;

    public GetDeparturesQueryHandler(ISwedaviaFlightApiService flightApiService)
    {
        _flightApiService = flightApiService;
    }

    public async Task<IEnumerable<FlightInfoDto>> Handle(GetDeparturesQuery request, CancellationToken cancellationToken)
    {
        return await _flightApiService.GetDeparturesAsync(
            request.FlightId ?? string.Empty,
            request.Airport,
            request.Date,
            cancellationToken);
    }
}
