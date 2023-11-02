using Microsoft.Extensions.Hosting;
using BoilerPlate.Core.Extensions;

namespace BoilerPlate.Services.Kafka.Utils;

public static class KafkaNameResolver
{
    public static string GetTopic(string baseTopic, IHostEnvironment environment)
    {
        if (environment.IsStaging())
        {
            return baseTopic + ".stage";
        }

        if (environment.IsDevelopment())
        {
            return baseTopic + ".dev";
        }

        if (environment.IsLocal())
        {
            return baseTopic + ".local";
        }

        return baseTopic;
    }

    public static string GetClientGroupId(string consumingTopic) => consumingTopic + ".group";
}