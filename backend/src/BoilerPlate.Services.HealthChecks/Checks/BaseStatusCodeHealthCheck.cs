using System.Net;
using System.Net.Sockets;
using BoilerPlate.Core.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace BoilerPlate.Services.HealthChecks.Checks;

public abstract class BaseStatusCodeHealthCheck : IHealthCheck
{
    protected abstract string Uri { get; }

    private readonly HttpHandler _httpHandler = new();

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken ct = default)
    {
        try
        {
            using var response = await _httpHandler.GetAsync(Uri, ct);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return HealthCheckResult.Healthy();
            }
        }
        catch (HttpRequestException e) when (e is { InnerException: SocketException })
        {
            // pass
        }

        return HealthCheckResult.Unhealthy();
    }
}