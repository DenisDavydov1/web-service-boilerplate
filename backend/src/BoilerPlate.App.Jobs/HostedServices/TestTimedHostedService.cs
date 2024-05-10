using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BoilerPlate.App.Jobs.HostedServices;

public class TestTimedHostedService(ILogger<TestTimedHostedService> logger) : IHostedService, IDisposable
{
    private int _executionCount;
    private Timer? _timer;

    public void Dispose() => _timer?.Dispose();

    public Task StartAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("TestTimedHostedService running");
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("TestTimedHostedService is stopping");
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    private void DoWork(object? state)
    {
        var count = Interlocked.Increment(ref _executionCount);
        logger.LogInformation("TestTimedHostedService is working. Count: {Count}", count);
    }
}