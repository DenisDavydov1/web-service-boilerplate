using BoilerPlate.Core.Options;

namespace BoilerPlate.Services.HealthChecks.Options.Services;

public class NginxHealthChecksServiceOptions : BasePolymorphicArrayElementOptions
{
    public string Uri { get; set; } = null!;
}