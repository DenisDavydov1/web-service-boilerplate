using BoilerPlate.Core.Options;

namespace BoilerPlate.Services.HealthChecks.Options.Publishers;

public class BaseHealthChecksPublisherOptions : BasePolymorphicArrayElementOptions
{
    public bool PublishOnlyErrors { get; set; }
}