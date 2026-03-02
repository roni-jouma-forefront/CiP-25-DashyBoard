using DashyBoard.Application.DTOs.Swedavia;

namespace DashyBoard.Application.Common.Interfaces.External;

public interface ISwedaviaFlightApiService
{
    Task<IEnumerable<FlightInfoDto>> GetArrivalsAsync(
        string flightId,
        string airportIATA,
        DateOnly date,
        CancellationToken cancellationToken = default
    );
    Task<IEnumerable<FlightInfoDto>> GetDeparturesAsync(
        string flightId,
        string airportIATA,
        DateOnly date,
        CancellationToken cancellationToken = default
    );
}
