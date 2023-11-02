using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BoilerPlate.App.Jobs.HostedServices;

public class TestTimedHostedService : IHostedService, IDisposable
{
    private readonly ILogger<TestTimedHostedService> _logger;
    private int _executionCount;
    private Timer? _timer;

    public TestTimedHostedService(ILogger<TestTimedHostedService> logger) => _logger = logger;

    public void Dispose() => _timer?.Dispose();

    public Task StartAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("TestTimedHostedService running");
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("TestTimedHostedService is stopping");
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    private void DoWork(object? state)
    {
        var count = Interlocked.Increment(ref _executionCount);
        _logger.LogInformation("TestTimedHostedService is working. Count: {Count}", count);
    }
}