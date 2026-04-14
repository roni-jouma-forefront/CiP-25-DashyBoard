using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DashyBoard.API.Controllers.ExternalControllers;

/// <summary>
/// Proxies the GitHub OAuth2 token exchange to bypass browser CORS restrictions.
/// Swagger UI calls this endpoint instead of GitHub's token URL directly.
/// </summary>
[ApiController]
[Route("oauth/github")]
[ApiExplorerSettings(IgnoreApi = true)]
[AllowAnonymous]
public class GitHubOAuthController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public GitHubOAuthController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    [HttpPost("token")]
    [Consumes("application/x-www-form-urlencoded")]
    public async Task<IActionResult> ExchangeToken([FromForm] IFormCollection form)
    {
        var clientId = _configuration["GitHub:ClientId"];
        var clientSecret = _configuration["GitHub:ClientSecret"];

        if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret))
            return StatusCode(500, new { error = "GitHub OAuth is not configured" });

        var formParams = new Dictionary<string, string>
        {
            { "client_id", clientId },
            { "client_secret", clientSecret },
            { "code", form["code"].ToString() },
            { "redirect_uri", form["redirect_uri"].ToString() },
        };

        if (!string.IsNullOrEmpty(form["code_verifier"]))
            formParams["code_verifier"] = form["code_verifier"].ToString();

        var request = new HttpRequestMessage(
            HttpMethod.Post,
            "https://github.com/login/oauth/access_token"
        )
        {
            Content = new FormUrlEncodedContent(formParams),
        };
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var client = _httpClientFactory.CreateClient();
        var response = await client.SendAsync(request);
        var content = await response.Content.ReadAsStringAsync();

        return Content(content, "application/json");
    }
}
