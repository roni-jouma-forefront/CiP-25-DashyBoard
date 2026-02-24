using System.Text.Json;
using DashyBoard.Application.Common.Interfaces.External;
using DashyBoard.Application.DTOs.Swedavia;
using DashyBoard.Domain.Entities.ExternalEntities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DashyBoard.Infrastructure.Services.External;

public class SwedaviaFlightApiService : SwedaviaApiServiceBase, ISwedaviaFlightApiService
{
    public SwedaviaFlightApiService(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<SwedaviaFlightApiService> logger)
        : base(
            httpClient,
            configuration["Swedavia:FlightInfoApiKey"]
                ?? throw new InvalidOperationException("Swedavia FlightInfo API key is not configured"),
            logger)
    {
    }

    public async Task<IEnumerable<FlightInfoDto>> GetArrivalsAsync(
        string flightId,
        string airportIATA,
        DateOnly date,
        CancellationToken cancellationToken = default)
    {
        return await GetFlightsAsync("arrivals", flightId, airportIATA, date, cancellationToken);
    }

    public async Task<IEnumerable<FlightInfoDto>> GetDeparturesAsync(
        string flightId,
        string airportIATA,
        DateOnly date,
        CancellationToken cancellationToken = default)
    {
        return await GetFlightsAsync("departures", flightId, airportIATA, date, cancellationToken);
    }

    private async Task<IEnumerable<FlightInfoDto>> GetFlightsAsync(
        string flightType,
        string flightId,
        string airportIATA,
        DateOnly date,
        CancellationToken cancellationToken)
    {
        var dateString = date.ToString("yyyy-MM-dd");
        var airportCode = airportIATA.ToUpperInvariant();
        var endpoint = BuildEndpoint("flightinfo/v2", airportCode, flightType, dateString);

        Logger.LogInformation(
            "Fetching {FlightType} for airport {Airport} on {Date}",
            flightType,
            airportCode,
            dateString);

        var flightInfoResponse = await SendApiRequestAsync<FlightInfoResponse>(endpoint, cancellationToken);
        var allFlights = MapToFlightInfoDtos(flightInfoResponse?.Flights ?? new List<FlightInfoApiModel>());

        // Filter by flightId if provided
        if (!string.IsNullOrWhiteSpace(flightId))
        {
            var upperFlightId = flightId.ToUpperInvariant();
            var filteredFlights = allFlights.Where(f => f.FlightId.ToUpperInvariant() == upperFlightId).ToList();

            Logger.LogInformation(
                "Filtered {TotalCount} {FlightType} to {FilteredCount} matching flight ID {FlightId}",
                allFlights.Count(),
                flightType,
                filteredFlights.Count,
                flightId);

            return filteredFlights;
        }

        Logger.LogInformation("Retrieved {Count} {FlightType} for {Airport}",
            allFlights.Count(), flightType, airportCode);

        return allFlights;
    }

    private static IEnumerable<FlightInfoDto> MapToFlightInfoDtos(IEnumerable<FlightInfoApiModel> apiModels)
    {
        return apiModels.Select(flight => new FlightInfoDto
        {
            FlightId = flight.FlightId ?? string.Empty,
            DepartureAirportIcao = flight.FlightLegIdentifier?.DepartureAirportIcao,
            ArrivalAirportIcao = flight.FlightLegIdentifier?.ArrivalAirportIcao,
            LocationAndStatus = flight.LocationAndStatus != null ? new LocationAndStatusDto
            {
                Terminal = flight.LocationAndStatus.Terminal,
                Gate = flight.LocationAndStatus.Gate,
                FlightLegStatusEnglish = flight.LocationAndStatus.FlightLegStatusEnglish
            } : null,
            ArrivalTime = flight.ArrivalTime != null ? new FlightTimeDto
            {
                EstimatedUtc = flight.ArrivalTime.EstimatedUtc,
                ScheduledUtc = flight.ArrivalTime.ScheduledUtc
            } : null,
            DepartureTime = flight.DepartureTime != null ? new FlightTimeDto
            {
                EstimatedUtc = flight.DepartureTime.EstimatedUtc,
                ScheduledUtc = flight.DepartureTime.ScheduledUtc
            } : null
        });
    }
}
