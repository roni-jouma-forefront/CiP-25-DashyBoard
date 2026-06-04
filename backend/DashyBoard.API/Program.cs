using DashyBoard.API.Authentication;
using DashyBoard.API.Converters;
using DashyBoard.API.Middleware;
using DashyBoard.Application;
using DashyBoard.Infrastructure;
using DashyBoard.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

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
// OpenTelemetry (Grafana Cloud + Local Prometheus)
// --------------------
var otelEndpoint = builder.Configuration["Grafana:OtlpEndpoint"];
var otelHeaders = builder.Configuration["Grafana:OtlpHeaders"];
var otelHost = string.IsNullOrWhiteSpace(otelEndpoint) ? null : new Uri(otelEndpoint).Host;

bool ShouldInstrumentOutgoingRequest(HttpRequestMessage request)
{
    // Prevent OTLP exporter HTTP calls from appearing as app outbound traffic.
    if (string.IsNullOrWhiteSpace(otelHost))
    {
        return true;
    }

    return !string.Equals(request.RequestUri?.Host, otelHost, StringComparison.OrdinalIgnoreCase);
}

// Export application logs to Grafana Cloud via OTLP
if (!string.IsNullOrEmpty(otelEndpoint))
{
    builder.Logging.AddOpenTelemetry(options =>
    {
        options.IncludeFormattedMessage = true;
        options.IncludeScopes = true;
        options.ParseStateValues = true;
        options.SetResourceBuilder(
            ResourceBuilder
                .CreateDefault()
                .AddService(
                    serviceName: "DashyBoard.API",
                    serviceVersion: "1.0.0",
                    serviceInstanceId: Environment.MachineName
                )
                .AddAttributes(
                    new Dictionary<string, object>
                    {
                        ["deployment.environment"] = builder.Environment.EnvironmentName,
                        ["host.name"] = Environment.MachineName,
                    }
                )
        );

        options.AddOtlpExporter(exporter =>
        {
            exporter.Endpoint = new Uri($"{otelEndpoint}/v1/logs");
            exporter.Headers = otelHeaders;
            exporter.Protocol = OtlpExportProtocol.HttpProtobuf;
        });
    });
}

builder
    .Services.AddOpenTelemetry()
    .ConfigureResource(resource =>
        resource
            .AddService(
                serviceName: "DashyBoard.API",
                serviceVersion: "1.0.0",
                serviceInstanceId: Environment.MachineName
            )
            .AddAttributes(
                new Dictionary<string, object>
                {
                    ["deployment.environment"] = builder.Environment.EnvironmentName,
                    ["host.name"] = Environment.MachineName,
                }
            )
    )
    .WithMetrics(metrics =>
    {
        metrics
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddRuntimeInstrumentation()
            .AddMeter("Npgsql"); // Database metrics for Neon/PostgreSQL

        // Export to Grafana Cloud if configured
        if (!string.IsNullOrEmpty(otelEndpoint))
        {
            metrics.AddOtlpExporter(options =>
            {
                options.Endpoint = new Uri($"{otelEndpoint}/v1/metrics");
                options.Headers = otelHeaders;
                options.Protocol = OtlpExportProtocol.HttpProtobuf;
            });
        }
    })
    .WithTracing(tracing =>
    {
        tracing
            .AddAspNetCoreInstrumentation(options =>
            {
                options.RecordException = true;
                options.Filter = ctx => !ctx.Request.Path.StartsWithSegments("/health");
            })
            .AddHttpClientInstrumentation(options =>
            {
                options.FilterHttpRequestMessage = ShouldInstrumentOutgoingRequest;
            })
            .AddEntityFrameworkCoreInstrumentation(options =>
            {
                options.SetDbStatementForText = true;
            })
            .AddSource("DashyBoard.API");

        // Export to Grafana Cloud if configured
        if (!string.IsNullOrEmpty(otelEndpoint))
        {
            tracing.AddOtlpExporter(options =>
            {
                options.Endpoint = new Uri($"{otelEndpoint}/v1/traces");
                options.Headers = otelHeaders;
                options.Protocol = OtlpExportProtocol.HttpProtobuf;
            });
        }
    });

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
