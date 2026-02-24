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
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Database
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        // Repositories
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        // Services
        services.AddTransient<IDateTime, DateTimeService>();

        // HTTP Clients with Polly retry policies
        services.AddHttpClient<ISwedaviaFlightApiService, SwedaviaFlightApiService>()
            .AddPolicyHandler(GetRetryPolicy());

        services.AddHttpClient<ISwedaviaWaitTimeApiService, SwedaviaWaitTimeApiService>()
            .AddPolicyHandler(GetRetryPolicy());

        services.AddHttpClient<ICheckWxApiService, CheckWxApiService>()
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
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), // Exponential backoff: 2, 4, 8 seconds
                onRetry: (outcome, timespan, retryAttempt, context) =>
                {
                    // Log retry attempts (will be picked up by the logger in the service)
                    Console.WriteLine($"Retry {retryAttempt} after {timespan.TotalSeconds}s due to: {outcome.Exception?.Message ?? outcome.Result.StatusCode.ToString()}");
                });
    }
}
