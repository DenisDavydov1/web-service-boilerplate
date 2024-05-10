using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using BoilerPlate.Services.Kafka.Options;

namespace BoilerPlate.Services.Kafka.AdminClient;

internal class KafkaAdminClient(ILogger<KafkaAdminClient> logger, IOptions<KafkaOptions> kafkaOptions)
    : IKafkaAdminClient
{
    private readonly KafkaOptions _kafkaOptions = kafkaOptions.Value;

    public async Task CreateTopicAsync(string name, int partitions, int retries = 10, CancellationToken ct = default)
    {
        Exception ex = null!;

        while (retries > 0 && !ct.IsCancellationRequested)
        {
            try
            {
                using var adminClient = new AdminClientBuilder(
                    new AdminClientConfig { BootstrapServers = _kafkaOptions.Servers }).Build();

                var isTopicExist = adminClient.GetMetadata(TimeSpan.FromSeconds(2)).Topics.Any(t => t.Topic == name);
                if (isTopicExist)
                {
                    return;
                }

                await adminClient.CreateTopicsAsync(new TopicSpecification[]
                {
                    new()
                    {
                        Name = name,
                        ReplicationFactor = _kafkaOptions.AdminReplicationFactor,
                        NumPartitions = partitions
                    }
                });

                logger.LogInformation("Topic {Name} created", name);
                return;
            }
            catch (Exception e)
            {
                await Task.Delay(5000, ct);
                ex = e;
                retries--;
                logger.LogWarning("Retrying to create topic {Name}...", name);
            }
        }

        throw new AggregateException($"Failed to create topic {name}", ex);
    }
}