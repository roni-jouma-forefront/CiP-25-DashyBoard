using System.Net.Http.Json;
using System.Text.RegularExpressions;
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
        ILogger<CheckWxApiService> logger
    )
    {
        _httpClient = httpClient;
        _logger = logger;
        _apiKey =
            configuration["CheckWx:ApiKey"]
            ?? throw new InvalidOperationException("CheckWx API key not configured");

        _httpClient.BaseAddress = new Uri("https://api.checkwx.com/");
        _httpClient.DefaultRequestHeaders.Add("X-API-Key", _apiKey);
    }

    public async Task<IEnumerable<CheckWxDto>> GetCurrentWeatherAsync(
        string icao,
        CancellationToken cancellationToken
    )
    {
        try
        {
            _logger.LogInformation("Fetching weather data for ICAO: {Icao}", icao);

            var apiResponse = await _httpClient.GetFromJsonAsync<CheckWxApiResponse>(
                $"v2/metar/{icao}/decoded",
                cancellationToken
            );

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
            Station =
                data.Station != null
                    ? new StationDto { Name = data.Station.Name, Location = data.Station.Location }
                    : null,
            Temperature =
                data.Temperature != null
                    ? new TemperatureDto
                    {
                        Celsius = data.Temperature.Celsius,
                        Fahrenheit = data.Temperature.Fahrenheit,
                    }
                    : null,
            Humidity = data.Humidity,
            WindSpeedMps = data.Wind?.Speed?.Mps,
            Weather = ParseWeather(data.RawText, data.Conditions),
        });
    }

    private static readonly Regex WeatherTokenRegex = new(
        @"^(-|\+|VC|RE)?(BC|BL|DR|FZ|MI|PR|SH|TS)?(RA|SN|DZ|GR|GS|IC|PE|BR|FG|DS|DU|FC|FU|HZ|PO|SA)+$",
        RegexOptions.Compiled
    );

    private static readonly Regex CloudTokenRegex = new(
        @"^(SKC|CLR|FEW|SCT|BKN|OVC)(\d{3})(CB|TCU)?$",
        RegexOptions.Compiled
    );

    private static readonly string[] CloudPriority = ["OVC", "BKN", "SCT", "FEW", "CLR", "SKC"];

    private static ParsedWeatherDto? ParseWeather(string? rawText, List<CheckWxConditionApi>? conditions)
    {
        string? snow = null;
        string? rain = null;
        string? fog = null;
        string? dominantCloud = null;

        if (!string.IsNullOrWhiteSpace(rawText))
        {
            foreach (var token in rawText.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            {
                if (WeatherTokenRegex.IsMatch(token))
                {
                    if (token.Contains("SN")) snow = token;
                    if (token.Contains("RA") || token.Contains("DZ")) rain = token;
                    if (token.Contains("FG") || token.Contains("BR")) fog = token;
                }

                var cloudMatch = CloudTokenRegex.Match(token);
                if (cloudMatch.Success)
                {
                    var cover = cloudMatch.Groups[1].Value;
                    if (dominantCloud == null ||
                        Array.IndexOf(CloudPriority, cover) < Array.IndexOf(CloudPriority, dominantCloud))
                    {
                        dominantCloud = cover;
                    }
                }
            }
        }

        if (snow == null && rain == null && fog == null && conditions != null)
        {
            foreach (var c in conditions)
            {
                var code = c.Code ?? string.Empty;
                if (code.Contains("SN")) snow = code;
                if (code.Contains("RA") || code.Contains("DZ")) rain = code;
                if (code.Contains("FG") || code.Contains("BR")) fog = code;
            }
        }

        if (snow == null && rain == null && fog == null && dominantCloud == null)
            return null;

        return new ParsedWeatherDto { Snow = snow, Rain = rain, Fog = fog, Cloud = dominantCloud };
    }
}
