using Confluent.Kafka;

namespace BoilerPlate.Services.Kafka.Options;

/// <summary>
/// Kafka settings
/// </summary>
public class KafkaOptions
{
    public const string SectionName = "Kafka";

    /// <summary> Connections to Kafka (usually with port 9092) </summary>
    public string Servers { get; set; } = null!;

    /// <summary> Log level for consumer </summary>
    public string ConsumerDebug { get; set; } = string.Empty;

    /// <summary> Log level for producer </summary>
    public string ProducerDebug { get; set; } = string.Empty;

    /// <summary> Topic to consume messages in hosted service </summary>
    public string ConsumerTopic { get; set; } = string.Empty;

    /// <summary> Replication factor for new topics created by admin client </summary>
    public short AdminReplicationFactor { get; set; }

    /// <summary> To Kafka builder consumer configuration </summary>
    public ConsumerConfig ToConsumerConfig()
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = Servers,
            Debug = ConsumerDebug,
            AutoOffsetReset = AutoOffsetReset.Earliest,
        };

        return config;
    }

    /// <summary> To Kafka builder producer configuration </summary>
    public ProducerConfig ToProducerConfig()
    {
        var config = new ProducerConfig
        {
            BootstrapServers = Servers,
            Debug = ProducerDebug,
        };

        return config;
    }
}