using System.Net.Http.Json;
using DashyBoard.Application.Common.Interfaces.External;
using DashyBoard.Application.DTOs.CheckWX;
using DashyBoard.Domain.Entities.ExternalEntities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DashyBoard.Infrastructure.Services.External;

public class CheckWxApiService : ICheckWxApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CheckWxApiService> _logger;
    private readonly string _apiKey;

    public CheckWxApiService(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<CheckWxApiService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _apiKey = configuration["CheckWx:ApiKey"]
            ?? throw new InvalidOperationException("CheckWx API key not configured");

        _httpClient.BaseAddress = new Uri("https://api.checkwx.com/");
        _httpClient.DefaultRequestHeaders.Add("X-API-Key", _apiKey);
    }

    public async Task<IEnumerable<CheckWxDto>> GetCurrentWeatherAsync(string icao, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Fetching weather data for ICAO: {Icao}", icao);

            var apiResponse = await _httpClient.GetFromJsonAsync<CheckWxApiResponse>(
                $"v2/metar/{icao}/decoded",
                cancellationToken);

            _logger.LogInformation("Successfully retrieved weather data for ICAO: {Icao}", icao);
            return MapToCheckWxDtos(apiResponse?.Data ?? new List<CheckWxApiData>());
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP error while fetching weather data for ICAO: {Icao}", icao);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching weather data for ICAO: {Icao}", icao);
            throw;
        }
    }

    private static IEnumerable<CheckWxDto> MapToCheckWxDtos(List<CheckWxApiData> apiData)
    {
        return apiData.Select(data => new CheckWxDto
        {
            Icao = data.Icao,
            Observed = data.Observed,
            Station = data.Station != null ? new StationDto
            {
                Name = data.Station.Name,
                Location = data.Station.Location
            } : null,
            Temperature = data.Temperature != null ? new TemperatureDto
            {
                Celsius = data.Temperature.Celsius,
                Fahrenheit = data.Temperature.Fahrenheit
            } : null,
            Humidity = data.Humidity,
            WindSpeedMps = data.Wind?.Speed?.Mps,
            Conditions = data.Conditions?.Select(c => new WeatherDto
            {
                Code = c.Code ?? string.Empty,
                Text = c.Text
            }).ToList()
        });
    }
}