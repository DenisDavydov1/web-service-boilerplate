using BoilerPlate.Core.Options;

namespace BoilerPlate.Services.HealthChecks.Options.Services;

public class ElasticsearchHealthChecksServiceOptions : BasePolymorphicArrayElementOptions
{
    public string Uri { get; set; } = null!;
}