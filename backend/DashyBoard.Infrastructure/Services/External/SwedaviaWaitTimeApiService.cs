using System.Net.Http.Headers;
using System.Text.Json;
using DashyBoard.Application.Common.Interfaces.External;
using DashyBoard.Application.DTOs.Swedavia;
using DashyBoard.Domain.Entities.ExternalEntities;
using Microsoft.Extensions.Configuration;

namespace DashyBoard.Infrastructure.Services.External;

public class SwedaviaWaitTimeApiService : ISwedaviaWaitTimeApiService
{
    private readonly HttpClient _httpClient;
    private readonly string _waitTimeApiKey;

    public SwedaviaWaitTimeApiService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://api.swedavia.se/");

        _waitTimeApiKey = configuration["Swedavia:WaitTimeApiKey"]
            ?? throw new InvalidOperationException("Swedavia WaitTime API key is not configured");
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
}
