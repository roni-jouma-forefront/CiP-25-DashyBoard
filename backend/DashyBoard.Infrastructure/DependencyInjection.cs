using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Application.Common.Interfaces.External;
using DashyBoard.Infrastructure.Persistence;
using DashyBoard.Infrastructure.Repositories;
using DashyBoard.Infrastructure.Services;
using DashyBoard.Infrastructure.Services.External;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
        
        // HTTP Clients
        services.AddHttpClient<ISwedaviaFlightApiService, SwedaviaFlightApiService>();
        services.AddHttpClient<ISwedaviaWaitTimeApiService, SwedaviaWaitTimeApiService>();

        return services;
    }
}
