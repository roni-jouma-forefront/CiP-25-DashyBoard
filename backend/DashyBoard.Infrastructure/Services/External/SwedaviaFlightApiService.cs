using System.Net.Http.Headers;
using System.Text.Json;
using DashyBoard.Application.Common.Interfaces.External;
using DashyBoard.Application.DTOs.Swedavia;
using DashyBoard.Domain.Entities.ExternalEntities;
using Microsoft.Extensions.Configuration;

namespace DashyBoard.Infrastructure.Services.External;

public class SwedaviaFlightApiService : ISwedaviaFlightApiService
{
    private readonly HttpClient _httpClient;
    private readonly string _flightInfoApiKey;
    private readonly string _waitTimeApiKey;

    public SwedaviaFlightApiService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://api.swedavia.se/");
        
        _flightInfoApiKey = configuration["Swedavia:FlightInfoApiKey"] 
            ?? throw new InvalidOperationException("Swedavia FlightInfo API key is not configured");
        
        _waitTimeApiKey = configuration["Swedavia:WaitTimeApiKey"] 
            ?? throw new InvalidOperationException("Swedavia WaitTime API key is not configured");
    }

    public async Task<IEnumerable<FlightInfoDto>> GetArrivalsAsync(
        string airportIATA, 
        DateOnly date, 
        CancellationToken cancellationToken = default)
    {
        var dateString = date.ToString("yyyy-MM-dd");
        
        using var request = new HttpRequestMessage(HttpMethod.Get, $"flightinfo/v2/{airportIATA}/arrivals/{dateString}");
        request.Headers.Add("Ocp-Apim-Subscription-Key", _flightInfoApiKey);
        
        var response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        var flightInfoResponse = JsonSerializer.Deserialize<FlightInfoResponse>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return MapToFlightInfoDtos(flightInfoResponse?.Flights ?? new List<FlightInfoApiModel>());
    }

    public async Task<IEnumerable<WaitTimeDto>> GetWaitTimesAsync(
        string airport, 
        string? flightId = null, 
        DateOnly? date = null, 
        CancellationToken cancellationToken = default)
    {
        var queryParams = new List<string>();
        if (!string.IsNullOrEmpty(flightId))
            queryParams.Add($"flightid={flightId}");
        if (date.HasValue)
            queryParams.Add($"date={date.Value:yyyy-MM-dd}");

        var queryString = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : string.Empty;
        
        using var request = new HttpRequestMessage(HttpMethod.Get, $"waittimepublic/v2/airports/{airport}/flights{queryString}");
        request.Headers.Add("Ocp-Apim-Subscription-Key", _waitTimeApiKey);
        
        var response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        var waitTimes = JsonSerializer.Deserialize<List<WaitTimeApiModel>>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return MapToWaitTimeDtos(waitTimes ?? new List<WaitTimeApiModel>());
    }

    private static IEnumerable<FlightInfoDto> MapToFlightInfoDtos(IEnumerable<FlightInfoApiModel> apiModels)
    {
        return apiModels.Select(flight => new FlightInfoDto
        {
            FlightId = flight.FlightId ?? string.Empty,
            LocationAndStatus = flight.LocationAndStatus != null ? new LocationAndStatusDto
            {
                Gate = flight.LocationAndStatus.Gate,
                FlightLegStatusEnglish = flight.LocationAndStatus.FlightLegStatusEnglish
            } : null,
            ArrivalTime = flight.ArrivalTime != null ? new ArrivalTimeDto
            {
                EstimatedUtc = flight.ArrivalTime.EstimatedUtc
            } : null
        });
    }

    private static IEnumerable<WaitTimeDto> MapToWaitTimeDtos(IEnumerable<WaitTimeApiModel> apiModels)
    {
        return apiModels.Select(wt => new WaitTimeDto
        {
            Airport = wt.Airport ?? string.Empty,
            FlightId = wt.FlightId,
            Date = wt.Date,
            WaitTimeMinutes = wt.WaitTimeMinutes
        });
    }

    //response modell from external api, so make n ew class library named something like airport data, integration, make sure its clear that its clear
    //map like responsemodels, some kind of api fetcher service that gets this then maps it to internal object
    //with what were interested in. so get intern object we can play with, then map to dtos for api response. so we have a clear separation between external api models and internal models.
    //keep response models separate from extern models. external class library uses http client
}

