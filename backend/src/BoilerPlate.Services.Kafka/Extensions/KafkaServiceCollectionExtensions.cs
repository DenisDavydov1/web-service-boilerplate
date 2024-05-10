using BoilerPlate.Core.Extensions;
using BoilerPlate.Services.Kafka.AdminClient;
using BoilerPlate.Services.Kafka.Consumer;
using BoilerPlate.Services.Kafka.Options;
using BoilerPlate.Services.Kafka.Producer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BoilerPlate.Services.Kafka.Extensions;

public static class KafkaServiceCollectionExtensions
{
    public static void AddKafka(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddServiceOptions<KafkaOptions>(configuration);
        services.AddTransient<IKafkaAdminClient, KafkaAdminClient>();
        services.AddTransient<IKafkaProducer, KafkaProducer>();
        services.AddHostedServiceAsSingleton<KafkaConsumerHostedService>();
    }
}