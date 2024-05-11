using System.Text;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using BoilerPlate.Data.Abstractions.Enums;
using BoilerPlate.Services.Kafka.Constants;
using BoilerPlate.Services.Kafka.Extensions;
using BoilerPlate.Services.Kafka.Options;
using BoilerPlate.Services.Kafka.Serializers;
using BoilerPlate.Services.Kafka.Utils;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BoilerPlate.Services.Kafka.Producer;

internal class KafkaProducer(
    IOptions<KafkaOptions> kafkaOptions,
    IHostEnvironment environment,
    ILogger<KafkaProducer> logger)
    : IKafkaProducer
{
    private readonly ProducerConfig _producerConfig = kafkaOptions.Value.ToProducerConfig();

    public async Task ProduceAsync(string baseTopic, int? partition, KafkaMessageType type, object? payload,
        CancellationToken ct = default)
    {
        var topic = KafkaNameResolver.GetTopic(baseTopic, environment);

        var id = Guid.NewGuid();
        var message = new Message<Null, object?>
        {
            Value = payload,
            Headers = new Headers
            {
                { MessageHeaders.MessageId, id.ToByteArray() },
                { MessageHeaders.MessageType, Encoding.UTF8.GetBytes(type.ToString()) }
            }
        };

        using var producer = new ProducerBuilder<Null, object?>(_producerConfig)
            .SetValueSerializer(new ObjectToStringBytesSerializer())
            .SetLogHandler(LogHandler)
            .Build();

        if (partition != null)
        {
            await producer.ProduceAsync(new TopicPartition(topic, partition.Value), message, ct);
        }
        else
        {
            await producer.ProduceAsync(topic, message, ct);
        }

        // LogMessage(id, type, topic, partition, payload);
    }

    private void LogHandler(IProducer<Null, object?> producer, LogMessage message) =>
        logger.Log(message.Level.ToLogLevel(), "{Message}", message.Message);

    private void LogMessage(Guid id, KafkaMessageType type, string topic, int? partition, object? payload)
    {
        var payloadText = JsonConvert.SerializeObject(payload);

        if (payloadText.Length > 1000)
        {
            payloadText = payloadText[..500] + "..." + payloadText[^500..];
        }

        logger.LogInformation(
            "Message sent:\n" +
            "id: {Id}\n" +
            "type: {Type}\n" +
            "topic: {Topic}\n" +
            "partition: {Partition}\n" +
            "payload: {Payload}\n",
            id, type, topic, partition, payloadText);
    }
}