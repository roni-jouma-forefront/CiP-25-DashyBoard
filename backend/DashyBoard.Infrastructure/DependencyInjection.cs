using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Application.Common.Interfaces.External;
using DashyBoard.Infrastructure.Persistence;
using DashyBoard.Infrastructure.Repositories;
using DashyBoard.Infrastructure.Services;
using DashyBoard.Infrastructure.Services.External;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace DashyBoard.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        // Database (Turso/LibSQL)
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            if (connectionString != null && connectionString.Contains("libsql://"))
            {
                // Convert from "Data Source=libsql://host?authToken=xxx" or "libsql://host?authToken=xxx"
                // to "https://host/v2/pipeline;token" format required by BMDRM.LibSql.Core
                var libsqlUrl = connectionString
                    .Replace("Data Source=", "", StringComparison.OrdinalIgnoreCase)
                    .Replace("data Source=", "", StringComparison.OrdinalIgnoreCase)
                    .Trim();
                var uri = new Uri(libsqlUrl);
                var token =
                    System.Web.HttpUtility.ParseQueryString(uri.Query).Get("authToken") ?? "";
                var converted = $"https://{uri.Host}/v2/pipeline;{token}";

                options.UseLibSql(
                    converted,
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
                );
            }
            else if (connectionString != null && connectionString.Contains("/v2/pipeline"))
            {
                options.UseLibSql(
                    connectionString,
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
                );
            }
            else
            {
                options.UseSqlite(
                    connectionString,
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
                );
            }
        });

        services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetRequiredService<ApplicationDbContext>()
        );

        // Repositories
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        // Services
        services.AddTransient<IDateTime, DateTimeService>();
        services.AddScoped<IBookingCsvParser, BookingCsvParser>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        // HTTP Clients with Polly retry policies
        services
            .AddHttpClient<ISwedaviaFlightApiService, SwedaviaFlightApiService>()
            .AddPolicyHandler(GetRetryPolicy());

        services
            .AddHttpClient<ISwedaviaWaitTimeApiService, SwedaviaWaitTimeApiService>()
            .AddPolicyHandler(GetRetryPolicy());

        services
            .AddHttpClient<ICheckWxApiService, CheckWxApiService>()
            .AddPolicyHandler(GetRetryPolicy());

        return services;
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError() // 5xx and 408
            .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.TooManyRequests) // 429
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: retryAttempt =>
                    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), // Exponential backoff: 2, 4, 8 seconds
                onRetry: (outcome, timespan, retryAttempt, context) =>
                {
                    // Log retry attempts (will be picked up by the logger in the service)
                    Console.WriteLine(
                        $"Retry {retryAttempt} after {timespan.TotalSeconds}s due to: {outcome.Exception?.Message ?? outcome.Result.StatusCode.ToString()}"
                    );
                }
            );
    }
}
