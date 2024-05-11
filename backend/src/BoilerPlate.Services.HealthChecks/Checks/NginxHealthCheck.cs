using BoilerPlate.Services.HealthChecks.Options.Services;

namespace BoilerPlate.Services.HealthChecks.Checks;

public class NginxHealthCheck(NginxHealthChecksServiceOptions options) : BaseStatusCodeHealthCheck
{
    protected override string Uri => options.Uri;
}