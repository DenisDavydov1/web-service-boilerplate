using BoilerPlate.Core.Extensions;
using BoilerPlate.Core.Utils;
using BoilerPlate.Services.HealthChecks.Checks;
using BoilerPlate.Services.HealthChecks.Options;
using BoilerPlate.Services.HealthChecks.Options.Publishers;
using BoilerPlate.Services.HealthChecks.Options.Services;
using BoilerPlate.Services.HealthChecks.Publishers;
using BoilerPlate.Services.Kafka.Options;
using BoilerPlate.Services.Mail.Options;
using BoilerPlate.Services.Mail.Options.Servers;
using BoilerPlate.Services.Mail.SmtpService;
using BoilerPlate.Services.Telegram.Options;
using Confluent.Kafka;
using HealthChecks.Elasticsearch;
using HealthChecks.Kafka;
using HealthChecks.NpgSql;
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
        [
            ("Services",
            [
                ("postgres", typeof(PostgresHealthChecksServiceOptions)),
                ("elasticsearch", typeof(ElasticsearchHealthChecksServiceOptions)),
                ("kibana", typeof(KibanaHealthChecksServiceOptions)),
                ("kafka", typeof(KafkaHealthChecksServiceOptions)),
                ("kafka-ui", typeof(KafkaUiHealthChecksServiceOptions)),
                ("nginx", typeof(NginxHealthChecksServiceOptions)),
            ]),
            ("Publishers",
            [
                ("log", typeof(LogHealthChecksPublisherOptions)),
                ("telegram", typeof(TelegramHealthChecksPublisherOptions)),
                ("mail", typeof(MailHealthChecksPublisherOptions)),
            ])
        ]);

        var healthChecksBuilder = services.AddHealthChecks();

        foreach (var serviceOptions in healthChecksOptions.Services)
        {
            switch (serviceOptions.Type)
            {
                case "postgres":
                {
                    if (EnvUtils.IsDbInMemoryMode == false)
                    {
                        var options = (PostgresHealthChecksServiceOptions) serviceOptions;
                        var connectionString = configuration.GetConnectionString("Default")!;

                        services.AddSingleton<NpgSqlHealthCheck>(_ => new NpgSqlHealthCheck(
                            new NpgSqlHealthCheckOptions(connectionString)));

                        healthChecksBuilder.AddCheck<NpgSqlHealthCheck>(
                            name: options.Name ?? "postgres",
                            tags: ["services"],
                            timeout: TimeSpan.FromSeconds(options.Timeout));
                    }

                    break;
                }
                case "elasticsearch":
                {
                    var options = (ElasticsearchHealthChecksServiceOptions) serviceOptions;
                    var timeout = TimeSpan.FromSeconds(options.Timeout);

                    services.AddSingleton<ElasticsearchHealthCheck>(_ => new ElasticsearchHealthCheck(
                        new ElasticsearchOptions { RequestTimeout = timeout }
                            .UseServer(options.Uri)));

                    healthChecksBuilder.AddCheck<ElasticsearchHealthCheck>(
                        name: options.Name ?? "elasticsearch",
                        tags: ["services"],
                        timeout: timeout);

                    break;
                }
                case "kibana":
                {
                    var options = (KibanaHealthChecksServiceOptions) serviceOptions;
                    services.AddSingleton<KibanaHealthCheck>(_ => new KibanaHealthCheck(options));

                    healthChecksBuilder.AddCheck<KibanaHealthCheck>(
                        name: options.Name ?? "kibana",
                        tags: ["services"],
                        timeout: TimeSpan.FromSeconds(options.Timeout));

                    break;
                }
                case "kafka":
                {
                    if (configuration.IsServiceEnabled<KafkaOptions>())
                    {
                        var options = (KafkaHealthChecksServiceOptions) serviceOptions;
                        var kafkaOptions = configuration.GetServiceOptions<KafkaOptions>();

                        services.AddSingleton<KafkaHealthCheck>(_ => new KafkaHealthCheck(new KafkaHealthCheckOptions
                        {
                            Configuration = new ProducerConfig { BootstrapServers = kafkaOptions.Servers }
                        }));

                        healthChecksBuilder.AddCheck<KafkaHealthCheck>(
                            name: options.Name ?? "kafka",
                            tags: ["services"],
                            timeout: TimeSpan.FromSeconds(options.Timeout));
                    }

                    break;
                }
                case "kafka-ui":
                {
                    var options = (KafkaUiHealthChecksServiceOptions) serviceOptions;
                    services.AddSingleton<KafkaUiHealthCheck>(_ => new KafkaUiHealthCheck(options));

                    healthChecksBuilder.AddCheck<KafkaUiHealthCheck>(
                        name: options.Name ?? "kafka-ui",
                        tags: ["services"],
                        timeout: TimeSpan.FromSeconds(options.Timeout));

                    break;
                }
                case "nginx":
                {
                    var options = (NginxHealthChecksServiceOptions) serviceOptions;
                    services.AddSingleton<NginxHealthCheck>(_ => new NginxHealthCheck(options));

                    healthChecksBuilder.AddCheck<NginxHealthCheck>(
                        name: options.Name ?? "nginx",
                        tags: ["services"],
                        timeout: TimeSpan.FromSeconds(options.Timeout));

                    break;
                }
            }
        }

        services.Configure<HealthCheckPublisherOptions>(options =>
        {
            options.Delay = TimeSpan.FromSeconds(healthChecksOptions.Delay);
            options.Period = TimeSpan.FromSeconds(healthChecksOptions.Period);
            options.Timeout = TimeSpan.FromSeconds(healthChecksOptions.Timeout);
        });

        foreach (var publisherOptions in healthChecksOptions.Publishers)
        {
            switch (publisherOptions.Type)
            {
                case "log":
                {
                    var options = (LogHealthChecksPublisherOptions) publisherOptions;

                    services.AddSingleton<IHealthCheckPublisher, LogHealthChecksPublisher>(provider =>
                        new LogHealthChecksPublisher(options,
                            provider.GetRequiredService<ILogger<LogHealthChecksPublisher>>()));

                    break;
                }
                case "telegram":
                {
                    if (configuration.IsServiceEnabled<TelegramOptions>())
                    {
                        var options = (TelegramHealthChecksPublisherOptions) publisherOptions;

                        services.AddSingleton<IHealthCheckPublisher, TelegramHealthChecksPublisher>(provider =>
                            new TelegramHealthChecksPublisher(options,
                                provider.GetRequiredKeyedService<ITelegramBotClient>(options.BotName)));
                    }

                    break;
                }
                case "mail":
                {
                    if (configuration.IsServiceEnabled<MailOptions>())
                    {
                        var options = (MailHealthChecksPublisherOptions) publisherOptions;

                        services.AddSingleton<IHealthCheckPublisher, MailHealthChecksPublisher>(provider =>
                            new MailHealthChecksPublisher(options,
                                provider.GetRequiredKeyedService<IMailSmtpService>(options.SmtpServerName)));
                    }

                    break;
                }
            }
        }
    }

    public static void UseServicesHealthChecks(this IApplicationBuilder applicationBuilder)
    {
        applicationBuilder.UseHealthChecks("/health/backend",
            new HealthCheckOptions
            {
                Predicate = r => r.Tags.Contains("services") == false,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

        applicationBuilder.UseHealthChecks("/health/services",
            new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
    }
}