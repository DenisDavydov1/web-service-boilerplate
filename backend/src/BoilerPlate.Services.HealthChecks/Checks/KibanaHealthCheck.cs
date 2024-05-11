using BoilerPlate.Services.HealthChecks.Options.Services;

namespace BoilerPlate.Services.HealthChecks.Checks;

public class KibanaHealthCheck(KibanaHealthChecksServiceOptions options) : BaseStatusCodeHealthCheck
{
    protected override string Uri => options.Uri;
}