using BoilerPlate.Core.Options;

namespace BoilerPlate.Services.HealthChecks.Options.Services;

public class ElasticsearchHealthChecksServiceOptions : BasePolymorphicArrayElementOptions
{
    public string? Name { get; set; }

    public string Uri { get; set; } = null!;

    public int Timeout { get; set; } = 5;
}