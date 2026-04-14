using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace DashyBoard.API.Authentication;

public class GitHubTokenAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public GitHubTokenAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration)
        : base(options, logger, encoder)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue("Authorization", out var authHeader))
            return AuthenticateResult.NoResult();

        var headerValue = authHeader.ToString();
        if (!headerValue.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            return AuthenticateResult.NoResult();

        var token = headerValue["Bearer ".Length..].Trim();
        if (string.IsNullOrEmpty(token))
            return AuthenticateResult.NoResult();

        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        client.DefaultRequestHeaders.UserAgent.ParseAdd("DashyBoard/1.0");
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var response = await client.GetAsync("https://api.github.com/user");
        if (!response.IsSuccessStatusCode)
            return AuthenticateResult.Fail("Invalid GitHub token");

        var content = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(content);
        var user = doc.RootElement;

        var repo = _configuration["GitHub:Repository"];
        if (!string.IsNullOrEmpty(repo))
        {
            var username = user.TryGetProperty("login", out var l) ? l.GetString() : null;
            var collaboratorResponse = await client.GetAsync($"https://api.github.com/repos/{repo}/collaborators/{username}");
            if (!collaboratorResponse.IsSuccessStatusCode)
                return AuthenticateResult.Fail($"Not a collaborator on the required GitHub repository: {repo}");
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.TryGetProperty("id", out var id) ? id.ToString() : ""),
            new Claim(ClaimTypes.Name, user.TryGetProperty("login", out var login) ? login.GetString() ?? "" : ""),
        };

        if (user.TryGetProperty("email", out var email) && email.ValueKind != JsonValueKind.Null)
            claims.Add(new Claim(ClaimTypes.Email, email.GetString() ?? ""));

        if (user.TryGetProperty("name", out var name) && name.ValueKind != JsonValueKind.Null)
            claims.Add(new Claim("full_name", name.GetString() ?? ""));

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }
}
