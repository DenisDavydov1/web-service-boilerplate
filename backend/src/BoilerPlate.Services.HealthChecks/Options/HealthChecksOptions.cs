using BoilerPlate.Core.Options;

namespace BoilerPlate.Services.HealthChecks.Options;

public class HealthChecksOptions : IServiceOptions
{
    public static string SectionName => "HealthChecks";

    public bool Enabled { get; set; } = true;

    public int Delay { get; set; } = 60;

    public int Period { get; set; } = 30;

    public IEnumerable<BasePolymorphicArrayElementOptions> Services { get; set; } =
        Array.Empty<BasePolymorphicArrayElementOptions>();

    public IEnumerable<BasePolymorphicArrayElementOptions> Publishers { get; set; } =
        Array.Empty<BasePolymorphicArrayElementOptions>();
}