using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DashyBoard.API.Authentication;

/// <summary>
/// Dynamically sets the GitHub OAuth token URL to the current request's host.
/// This ensures the token proxy URL is correct regardless of environment.
/// </summary>
public class GitHubTokenUrlFilter : IDocumentFilter
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GitHubTokenUrlFilter(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var request = _httpContextAccessor.HttpContext?.Request;
        if (request is null)
            return;

        var baseUrl = $"{request.Scheme}://{request.Host}";

        if (swaggerDoc.Components.SecuritySchemes.TryGetValue("github", out var scheme))
        {
            scheme.Flows.AuthorizationCode.TokenUrl = new Uri($"{baseUrl}/oauth/github/token");
        }
    }
}
