using Microsoft.Extensions.Configuration;
using BoilerPlate.Services.Kafka.Options;

namespace BoilerPlate.Services.Kafka.Extensions;

public static class ConfigurationExtensions
{
    /// <summary> Run with kafka services </summary>
    public static bool IsKafkaEnabled(this IConfiguration configuration)
    {
        var section = configuration.GetSection(KafkaOptions.SectionName);
        return section.Exists() && section.Get<KafkaOptions>()?.Enabled == true;
    }
}