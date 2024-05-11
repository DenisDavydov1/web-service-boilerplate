using BoilerPlate.Core.Serialization;
using BoilerPlate.Services.HealthChecks.Options;
using BoilerPlate.Services.HealthChecks.Options.Publishers;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BoilerPlate.Services.HealthChecks.Publishers;

internal class LogHealthChecksPublisher(
    LogHealthChecksPublisherOptions publisherOptions,
    ILogger<LogHealthChecksPublisher> logger)
    : BaseHealthChecksPublisher<LogHealthChecksPublisherOptions>(publisherOptions)
{
    protected override Task PublishInternal(HealthReport report, CancellationToken ct)
    {
        logger.Log(
            report.Status == HealthStatus.Healthy ? LogLevel.Information : LogLevel.Error,
            "{Report}",
            JsonConvert.SerializeObject(report, SerializationSettings.Default));

        return Task.CompletedTask;
    }
}