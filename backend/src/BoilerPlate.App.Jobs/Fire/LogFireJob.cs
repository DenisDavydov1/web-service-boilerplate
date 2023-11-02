using Coravel.Invocable;
using Microsoft.Extensions.Logging;

namespace BoilerPlate.App.Jobs.Fire;

public class LogFireJob : IInvocable, IInvocableWithPayload<LogFireJobPayload>
{
    private readonly ILogger<LogFireJob> _logger;

    public LogFireJobPayload Payload { get; set; } = null!;

    public LogFireJob(ILogger<LogFireJob> logger) => _logger = logger;

    public Task Invoke()
    {
        _logger.LogInformation("{Job} invoked. Message: {Message}", nameof(LogFireJob), Payload.Message);
        return Task.CompletedTask;
    }

}