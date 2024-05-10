using System.Text;
using CaseExtensions;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using BoilerPlate.Data.Abstractions.Enums;
using BoilerPlate.Services.Kafka.Constants;
using BoilerPlate.Services.Kafka.Options;
using BoilerPlate.Services.Kafka.Serializers;
using BoilerPlate.Services.Kafka.Utils;

namespace BoilerPlate.Services.Kafka.Producer;

internal class KafkaProducer(IOptions<KafkaOptions> kafkaOptions, IHostEnvironment environment)
    : IKafkaProducer
{
    private readonly ProducerConfig _producerConfig = kafkaOptions.Value.ToProducerConfig();

    public async Task ProduceAsync(string baseTopic, int? partition, KafkaMessageType type, object? payload,
        CancellationToken ct = default)
    {
        var topic = KafkaNameResolver.GetTopic(baseTopic, environment);

        var message = new Message<Null, object?>
        {
            Value = payload,
            Headers = new Headers
            {
                { MessageHeaders.MessageId, Guid.NewGuid().ToByteArray() },
                { MessageHeaders.MessageType, Encoding.UTF8.GetBytes(type.ToString()) }
            }
        };

        using var producer = new ProducerBuilder<Null, object?>(_producerConfig)
            .SetValueSerializer(new ObjectToStringBytesSerializer())
            .Build();

        if (partition != null)
        {
            await producer.ProduceAsync(new TopicPartition(topic, partition.Value), message, ct);
        }
        else
        {
            await producer.ProduceAsync(topic, message, ct);
        }
    }
}