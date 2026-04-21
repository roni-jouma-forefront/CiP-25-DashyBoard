using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DashyBoard.Application.Services;

public class TimedTriggerService : BackgroundService
{
    private Timer? _timer;
    private readonly ILogger<TimedTriggerService> _logger;

    public TimedTriggerService(ILogger<TimedTriggerService> logger)
    {
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
        return Task.CompletedTask;
    }

    private void DoWork(object? state)
    {
        // Implement the logic to be executed on each trigger here
        _logger.LogInformation("Timed trigger executed at: {Time}", DateTime.Now);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _timer?.Dispose();
        base.Dispose();
    }
}
