using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace DashyBoard.Infrastructure.Services.External;

public abstract class SwedaviaApiServiceBase
{
    protected readonly HttpClient HttpClient;
    protected readonly ILogger Logger;
    private readonly string _apiKey;

    protected SwedaviaApiServiceBase(HttpClient httpClient, string apiKey, ILogger logger)
    {
        HttpClient = httpClient;
        HttpClient.BaseAddress ??= new Uri("https://api.swedavia.se/");
        _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
        Logger = logger;
    }

    protected async Task<TResponse> SendApiRequestAsync<TResponse>(
        string endpoint,
        CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
        request.Headers.Add("Ocp-Apim-Subscription-Key", _apiKey);
        request.Headers.Add("Accept", "application/json");

        Logger.LogDebug("Sending request to Swedavia API: {Endpoint}", endpoint);

        HttpResponseMessage response;
        try
        {
            response = await HttpClient.SendAsync(request, cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            Logger.LogError(ex, "HTTP request failed for endpoint: {Endpoint}", endpoint);
            throw new InvalidOperationException($"Failed to communicate with Swedavia API: {ex.Message}", ex);
        }
        catch (TaskCanceledException ex)
        {
            Logger.LogWarning(ex, "Request to {Endpoint} was cancelled or timed out", endpoint);
            throw new InvalidOperationException("The request to Swedavia API timed out", ex);
        }

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
            Logger.LogError(
                "Swedavia API returned error {StatusCode} for endpoint {Endpoint}: {ErrorContent}",
                response.StatusCode,
                endpoint,
                errorContent);

            throw new HttpRequestException(
                $"Swedavia API returned {response.StatusCode}: {errorContent}");
        }

        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        try
        {
            var result = JsonSerializer.Deserialize<TResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (result == null)
            {
                Logger.LogWarning("Deserialization returned null for endpoint: {Endpoint}", endpoint);
                throw new InvalidOperationException("Failed to deserialize API response");
            }

            Logger.LogDebug("Successfully received and deserialized response from {Endpoint}", endpoint);
            return result;
        }
        catch (JsonException ex)
        {
            Logger.LogError(ex, "Failed to deserialize response from {Endpoint}. Content: {Content}",
                endpoint, content);
            throw new InvalidOperationException("Failed to parse API response", ex);
        }
    }

    protected static string BuildEndpoint(string basePath, params string[] segments)
    {
        var path = string.Join("/", segments.Where(s => !string.IsNullOrWhiteSpace(s)));
        return $"{basePath}/{path}";
    }
}
