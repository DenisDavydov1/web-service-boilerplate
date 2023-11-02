using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using BoilerPlate.Core.Extensions;
using BoilerPlate.Core.Utils;
using BoilerPlate.Data.Notifications.Attributes;
using BoilerPlate.Data.Notifications.Common;
using BoilerPlate.Services.Kafka.AdminClient;
using BoilerPlate.Services.Kafka.Extensions;
using BoilerPlate.Services.Kafka.Options;
using BoilerPlate.Services.Kafka.Serializers;
using BoilerPlate.Services.Kafka.Utils;

namespace BoilerPlate.Services.Kafka.Consumer;

public class KafkaConsumerHostedService : IHostedService, IDisposable
{
    private readonly ILogger<KafkaConsumerHostedService> _logger;
    private readonly IKafkaAdminClient _kafkaAdminClient;
    private readonly ConsumerConfig _consumerConfig;
    private IConsumer<Ignore, JObject?>? _consumer;
    private CancellationTokenSource? _cancellationTokenSource;
    private readonly IMediator _mediator;
    private readonly string _topic;

    public KafkaConsumerHostedService(ILogger<KafkaConsumerHostedService> logger, IOptions<KafkaOptions> kafkaOptions,
        IHostEnvironment environment, IKafkaAdminClient kafkaAdminClient, IMediator mediator)
    {
        _logger = logger;
        _kafkaAdminClient = kafkaAdminClient;
        _mediator = mediator;
        _topic = KafkaNameResolver.GetTopic(kafkaOptions.Value.ConsumerTopic, environment);
        _consumerConfig = kafkaOptions.Value.ToConsumerConfig();
        _consumerConfig.GroupId = KafkaNameResolver.GetClientGroupId(_topic);
    }

    public void Dispose()
    {
        try
        {
            _consumer?.Close();
            _cancellationTokenSource?.Cancel();
        }
        catch (ObjectDisposedException)
        {
            // ignored
        }

        _cancellationTokenSource?.Dispose();
        _consumer?.Dispose();
    }

    public async Task StartAsync(CancellationToken ct)
    {
        await _kafkaAdminClient.CreateTopicAsync(_topic, partitions: 1, ct: ct);

        _consumer = new ConsumerBuilder<Ignore, JObject?>(_consumerConfig)
            .SetValueDeserializer(new StringBytesToJObjectDeserializer())
            .SetErrorHandler(HandleConsumeError)
            .Build();
        _consumer.Subscribe(_topic);

        _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(ct);
        _logger.LogInformation("Kafka Consumer hosted service is running");
        Task.Run(Consume, ct).GetAwaiter();
    }

    public Task StopAsync(CancellationToken ct)
    {
        Dispose();
        _logger.LogInformation("Kafka Consumer hosted service stopped");
        return Task.CompletedTask;
    }

    private async Task Consume()
    {
        while (_cancellationTokenSource?.IsCancellationRequested == false)
        {
            try
            {
                var consumeResult = _consumer!.Consume(_cancellationTokenSource.Token);
                var message = consumeResult.Message;
                var messageId = message.Headers.GetMessageId();
                var messageType = message.Headers.GetMessageType();

                var notificationTypes = AssemblyUtils.GetTypes()
                    .Where(t =>
                        t.IsAbstract == false &&
                        t.GetCustomAttributes(true).OfType<KafkaMessageAttribute>().Any(a => a.Type == messageType));
                foreach (var type in notificationTypes)
                {
                    var args = new List<object> { messageId, messageType };
                    if (type.IsInheritedFromOpenGenericType(typeof(BaseKafkaMessageWithPayloadNotification<>)))
                    {
                        args.Add(message.Value!);
                    }

                    var notification = Activator.CreateInstance(type, args.ToArray());
                    await _mediator.Publish(notification!, _cancellationTokenSource.Token);
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Kafka consumer stop");
                return;
            }
            catch (ConsumeException ex)
            {
                _logger.LogError(ex, "Kafka consumer generated an exception");
            }
            catch (KafkaException ex)
            {
                _logger.LogError(ex, "Kafka generated an exception");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kafka caught unexpected exception");
            }
        }
    }

    private void HandleConsumeError(IConsumer<Ignore, JObject?> consumer, Error e)
        => _logger.LogCritical("Kafka consumer error: {Reason}", e.Reason);
}