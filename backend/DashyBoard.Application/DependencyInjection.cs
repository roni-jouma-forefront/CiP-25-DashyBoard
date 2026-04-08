using System.Reflection;
using DashyBoard.Application.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DashyBoard.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly())
        );
        services.AddHostedService<TimedTriggerService>();

        return services;
    }
}
