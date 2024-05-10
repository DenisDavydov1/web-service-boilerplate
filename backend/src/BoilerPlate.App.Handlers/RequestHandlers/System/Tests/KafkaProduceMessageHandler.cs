using MediatR;
using BoilerPlate.Data.DTO.System.Tests.Requests;
using BoilerPlate.Services.Kafka.Producer;

namespace BoilerPlate.App.Handlers.RequestHandlers.System.Tests;

public class KafkaProduceMessageHandler(IKafkaProducer producer) : IRequestHandler<KafkaProduceMessageDto>
{
    public async Task Handle(KafkaProduceMessageDto request, CancellationToken ct) =>
        await producer.ProduceAsync(request.Topic, request.Partition, request.Type, request.Payload, ct);
}