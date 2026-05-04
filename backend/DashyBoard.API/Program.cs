using DashyBoard.API.Authentication;
using DashyBoard.API.Converters;
using DashyBoard.API.Middleware;
using DashyBoard.Application;
using DashyBoard.Infrastructure;
using DashyBoard.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// --------------------
// Controllers / JSON
// --------------------
builder
    .Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();

// --------------------
// Swagger
// --------------------
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Title = "DashyBoard API",
            Version = "v1",
            Description = "API for managing DashyBoard application",
        }
    );

    c.AddSecurityDefinition(
        "github",
        new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.OAuth2,
            Flows = new OpenApiOAuthFlows
            {
                AuthorizationCode = new OpenApiOAuthFlow
                {
                    AuthorizationUrl = new Uri("https://github.com/login/oauth/authorize"),
                    TokenUrl = new Uri("https://placeholder/oauth/github/token"),
                    Scopes = new Dictionary<string, string>
                    {
                        { "read:user", "Read GitHub user profile" },
                        { "user:email", "Read GitHub user email" },
                        { "repo", "Check repository collaborator access" },
                    },
                },
            },
        }
    );

    c.DocumentFilter<GitHubTokenUrlFilter>();
});

// --------------------
// Authentication (NO JWT)
// --------------------
builder
    .Services.AddAuthentication("GitHub")
    .AddScheme<AuthenticationSchemeOptions, GitHubTokenAuthHandler>("GitHub", options => { });

builder.Services.AddAuthorization();

// --------------------
// CORS
// --------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        }
    );
});

// --------------------
// Health checks
// --------------------
builder.Services.AddHealthChecks();

// --------------------
// Clean Architecture layers
// --------------------
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// --------------------
// Turso-safe migrations
// --------------------
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    if (
        !context.Database.ProviderName?.Contains("InMemory", StringComparison.OrdinalIgnoreCase)
        ?? false
    )
    {
        context.Database.Migrate();
    }
}

// --------------------
// Middleware pipeline
// --------------------
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "DashyBoard API v1");
    c.OAuthClientId(builder.Configuration["GitHub:ClientId"]);
    c.OAuthUsePkce();
});

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();

public partial class Program { }
