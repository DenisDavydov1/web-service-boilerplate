using Coravel.Invocable;
using Microsoft.Extensions.Logging;

namespace BoilerPlate.App.Jobs.Fire;

public class LogFireJob(ILogger<LogFireJob> logger) : IInvocable, IInvocableWithPayload<LogFireJobPayload>
{
    public LogFireJobPayload Payload { get; set; } = null!;

    public Task Invoke()
    {
        logger.LogInformation("{Job} invoked. Message: {Message}", nameof(LogFireJob), Payload.Message);
        return Task.CompletedTask;
    }

}