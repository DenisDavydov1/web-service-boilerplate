using BoilerPlate.Data.Abstractions.Enums;

namespace BoilerPlate.Services.Kafka.Producer;

public interface IKafkaProducer
{
    Task ProduceAsync(string baseTopic, int? partition, KafkaMessageType type, object? payload, CancellationToken ct = default);
}