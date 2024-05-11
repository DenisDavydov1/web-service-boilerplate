using BoilerPlate.Services.HealthChecks.Options.Publishers;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace BoilerPlate.Services.HealthChecks.Publishers;

internal abstract class BaseHealthChecksPublisher<TPublisherOptions>(TPublisherOptions publisherOptions)
    : IHealthCheckPublisher
    where TPublisherOptions : BaseHealthChecksPublisherOptions
{
    protected readonly TPublisherOptions PublisherOptions = publisherOptions;

    public Task PublishAsync(HealthReport report, CancellationToken ct)
    {
        if (PublisherOptions.PublishOnlyErrors && report.Status == HealthStatus.Healthy)
        {
            return Task.CompletedTask;
        }

        return PublishInternal(report, ct);
    }

    protected abstract Task PublishInternal(HealthReport report, CancellationToken ct);
}