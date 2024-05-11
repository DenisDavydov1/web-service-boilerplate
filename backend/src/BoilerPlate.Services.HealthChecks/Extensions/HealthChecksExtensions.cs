using BoilerPlate.Core.Extensions;
using BoilerPlate.Core.Utils;
using BoilerPlate.Services.HealthChecks.Checks;
using BoilerPlate.Services.HealthChecks.Options;
using BoilerPlate.Services.HealthChecks.Options.Publishers;
using BoilerPlate.Services.HealthChecks.Options.Services;
using BoilerPlate.Services.HealthChecks.Publishers;
using BoilerPlate.Services.Kafka.Options;
using BoilerPlate.Services.Telegram.Options;
using Confluent.Kafka;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace BoilerPlate.Services.HealthChecks.Extensions;

public static class HealthChecksExtensions
{
    public static void AddServicesHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        var healthChecksOptions = services.AddServiceOptions<HealthChecksOptions>(configuration,
            new Dictionary<string, Dictionary<string, Type>>
            {
                { "Services", new Dictionary<string, Type>
                {
                    { "postgres", typeof(PostgresHealthChecksServiceOptions) },
                    { "elasticsearch", typeof(ElasticsearchHealthChecksServiceOptions) },
                    { "kibana", typeof(KibanaHealthChecksServiceOptions) },
                    { "kafka", typeof(KafkaHealthChecksServiceOptions) },
                    { "kafka-ui", typeof(KafkaUiHealthChecksServiceOptions) },
                    { "nginx", typeof(NginxHealthChecksServiceOptions) },
                } },
                { "Publishers", new Dictionary<string, Type>
                {
                    { "log", typeof(LogHealthChecksPublisherOptions) },
                    { "telegram", typeof(TelegramHealthChecksPublisherOptions) },
                    { "email", typeof(EmailHealthChecksPublisherOptions) },
                } }
            });

        var healthChecksBuilder = services.AddHealthChecks();

        foreach (var serviceOptions in healthChecksOptions.Services)
        {
            switch (serviceOptions.Type)
            {
                case "postgres":
                {
                    if (EnvUtils.IsDbInMemoryMode == false)
                    {
                        var connectionString = configuration.GetConnectionString("Default")!;
                        healthChecksBuilder.AddNpgSql(connectionString, name: "postgres");
                    }
                    break;
                }
                case "elasticsearch":
                {
                    healthChecksBuilder.AddElasticsearch(
                        ((ElasticsearchHealthChecksServiceOptions) serviceOptions).Uri);
                    break;
                }
                case "kibana":
                {
                    services.AddSingleton<KibanaHealthCheck>(_ =>
                        new KibanaHealthCheck((KibanaHealthChecksServiceOptions) serviceOptions));
                    healthChecksBuilder.AddCheck<KibanaHealthCheck>("kibana");
                    break;
                }
                case "kafka":
                {
                    if (configuration.IsServiceEnabled<KafkaOptions>())
                    {
                        var kafkaOptions = configuration.GetServiceOptions<KafkaOptions>();
                        healthChecksBuilder.AddKafka(new ProducerConfig
                        {
                            BootstrapServers = kafkaOptions.Servers
                        });
                    }
                    break;
                }
                case "kafka-ui":
                {
                    services.AddSingleton<KafkaUiHealthCheck>(_ =>
                        new KafkaUiHealthCheck((KafkaUiHealthChecksServiceOptions) serviceOptions));
                    healthChecksBuilder.AddCheck<KafkaUiHealthCheck>("kafka-ui");
                    break;
                }
                case "nginx":
                {
                    services.AddSingleton<NginxHealthCheck>(_ =>
                        new NginxHealthCheck((NginxHealthChecksServiceOptions) serviceOptions));
                    healthChecksBuilder.AddCheck<NginxHealthCheck>("nginx");
                    break;
                }
            }
        }

        services.Configure<HealthCheckPublisherOptions>(options =>
        {
            options.Delay = TimeSpan.FromSeconds(healthChecksOptions.Delay);
            options.Period = TimeSpan.FromSeconds(healthChecksOptions.Period);
            options.Timeout = Timeout.InfiniteTimeSpan;
        });

        foreach (var publisherOptions in healthChecksOptions.Publishers)
        {
            switch (publisherOptions.Type)
            {
                case "log":
                {
                    services.AddSingleton<IHealthCheckPublisher, LogHealthChecksPublisher>(provider =>
                        new LogHealthChecksPublisher(
                            (LogHealthChecksPublisherOptions) publisherOptions,
                            provider.GetRequiredService<ILogger<LogHealthChecksPublisher>>()));
                    break;
                }
                case "telegram":
                {
                    if (configuration.IsServiceEnabled<TelegramOptions>())
                    {
                        services.AddSingleton<IHealthCheckPublisher, TelegramHealthChecksPublisher>(provider =>
                            new TelegramHealthChecksPublisher(
                                (TelegramHealthChecksPublisherOptions) publisherOptions,
                                provider.GetRequiredService<ITelegramBotClient>()));
                    }

                    break;
                }
                case "email":
                {
                    break;
                }
            }
        }
    }

    public static void UseServicesHealthChecks(this IApplicationBuilder applicationBuilder) =>
        applicationBuilder.UseHealthChecks("/healthz", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
}