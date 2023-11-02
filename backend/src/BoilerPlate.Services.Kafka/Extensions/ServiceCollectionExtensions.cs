using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BoilerPlate.Core.Extensions;
using BoilerPlate.Services.Kafka.AdminClient;
using BoilerPlate.Services.Kafka.Consumer;
using BoilerPlate.Services.Kafka.Options;
using BoilerPlate.Services.Kafka.Producer;

namespace BoilerPlate.Services.Kafka.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddKafka(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<KafkaOptions>(configuration.GetSection(KafkaOptions.SectionName));
        services.AddTransient<IKafkaAdminClient, KafkaAdminClient>();
        services.AddTransient<IKafkaProducer, KafkaProducer>();
        services.AddHostedServiceAsSingleton<KafkaConsumerHostedService>();
    }
}