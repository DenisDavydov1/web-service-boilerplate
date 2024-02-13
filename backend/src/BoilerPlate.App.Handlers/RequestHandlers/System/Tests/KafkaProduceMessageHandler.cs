using MediatR;
using BoilerPlate.Data.DTO.System.Tests.Requests;
using BoilerPlate.Services.Kafka.Producer;

namespace BoilerPlate.App.Handlers.RequestHandlers.System.Tests;

public class KafkaProduceMessageHandler : IRequestHandler<KafkaProduceMessageDto>
{
    private readonly IKafkaProducer _producer;

    public KafkaProduceMessageHandler(IKafkaProducer producer) => _producer = producer;

    public async Task Handle(KafkaProduceMessageDto request, CancellationToken ct) =>
        await _producer.ProduceAsync(request.Topic, request.Partition, request.Type, request.Payload, ct);
}