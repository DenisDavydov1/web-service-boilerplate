using BoilerPlate.Core.Options;

namespace BoilerPlate.Services.HealthChecks.Options.Services;

public class KafkaHealthChecksServiceOptions : BasePolymorphicArrayElementOptions
{
    public string? Name { get; set; }

    public int Timeout { get; set; } = 5;
}