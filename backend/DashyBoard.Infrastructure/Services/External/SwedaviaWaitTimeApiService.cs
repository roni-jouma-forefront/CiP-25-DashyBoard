using System.Text.Json;
using DashyBoard.Application.Common.Interfaces.External;
using DashyBoard.Application.DTOs.Swedavia;
using DashyBoard.Domain.Entities.ExternalEntities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DashyBoard.Infrastructure.Services.External;

public class SwedaviaWaitTimeApiService : SwedaviaApiServiceBase, ISwedaviaWaitTimeApiService
{
    public SwedaviaWaitTimeApiService(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<SwedaviaWaitTimeApiService> logger)
        : base(
            httpClient,
            configuration["Swedavia:WaitTimeApiKey"]
                ?? throw new InvalidOperationException("Swedavia WaitTime API key is not configured"),
            logger)
    {
    }

    public async Task<IEnumerable<WaitTimeDto>> GetWaitTimesAsync(
        string airport,
        DateOnly? date = null,
        CancellationToken cancellationToken = default)
    {
        var airportCode = airport.ToUpperInvariant();
        var endpoint = BuildEndpoint("waittimepublic/v2/airports", airportCode);

        Logger.LogInformation(
            "Fetching wait times for airport {Airport}{DateInfo}",
            airportCode,
            date.HasValue ? $" on {date.Value:yyyy-MM-dd}" : " (current)");

        var airportWaitTimes = await SendApiRequestAsync<AirportWaitTimeResponse>(endpoint, cancellationToken);
        var waitTimes = MapToWaitTimeDtos(airportWaitTimes?.WaitTimes ?? new List<SecurityQueueWaitTime>());

        Logger.LogInformation(
            "Retrieved {Count} wait time entries for {Airport}",
            waitTimes.Count(),
            airportCode);

        return waitTimes;
    }

    private static IEnumerable<WaitTimeDto> MapToWaitTimeDtos(IEnumerable<SecurityQueueWaitTime> apiModels)
    {
        return apiModels.Select(wt => new WaitTimeDto
        {
            QueueName = wt.QueueName ?? string.Empty,
            Terminal = wt.Terminal ?? string.Empty,
            CurrentTime = wt.CurrentTime,
            CurrentProjectedWaitTime = wt.CurrentProjectedWaitTime,
            IsFastTrack = wt.IsFastTrack
        });
    }
}
