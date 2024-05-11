using BoilerPlate.Core.Extensions;
using BoilerPlate.Core.Utils;
using BoilerPlate.Data.Abstractions.Enums;
using BoilerPlate.Data.Notifications.Attributes;
using BoilerPlate.Data.Notifications.Common;
using BoilerPlate.Services.Kafka.AdminClient;
using BoilerPlate.Services.Kafka.Extensions;
using BoilerPlate.Services.Kafka.Options;
using BoilerPlate.Services.Kafka.Serializers;
using BoilerPlate.Services.Kafka.Utils;
using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
    private readonly Dictionary<KafkaMessageType, List<Type>> _notificationTypes;

    public KafkaConsumerHostedService(ILogger<KafkaConsumerHostedService> logger, IOptions<KafkaOptions> kafkaOptions,
        IHostEnvironment environment, IKafkaAdminClient kafkaAdminClient, IMediator mediator)
    {
        _logger = logger;
        _kafkaAdminClient = kafkaAdminClient;
        _mediator = mediator;
        _topic = KafkaNameResolver.GetTopic(kafkaOptions.Value.ConsumerTopic, environment);
        _consumerConfig = kafkaOptions.Value.ToConsumerConfig();
        _consumerConfig.GroupId = KafkaNameResolver.GetClientGroupId(_topic);
        _notificationTypes = GetKafkaNotificationTypesDictionary();
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
            .SetLogHandler(LogHandler)
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

                // LogMessage(messageId, messageType, consumeResult.Topic, consumeResult.Partition, message.Value);

                var notificationTypes = _notificationTypes.GetValueOrDefault(messageType) ?? new List<Type>();
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

    private static Dictionary<KafkaMessageType, List<Type>> GetKafkaNotificationTypesDictionary()
    {
        var notificationTypes = new Dictionary<KafkaMessageType, List<Type>>();

        foreach (var type in AssemblyUtils.GetTypes())
        {
            if (type.IsAbstract || !type.IsAssignableTo(typeof(INotification))) continue;

            var kafkaMsgTypes = type.GetCustomAttributes(false)
                .OfType<KafkaMessageAttribute>()
                .Select(x => x.Type);

            foreach (var kafkaMsgType in kafkaMsgTypes)
            {
                var isInDict = notificationTypes.TryGetValue(kafkaMsgType, out var msgTypeNotificationTypes);
                if (isInDict)
                {
                    msgTypeNotificationTypes!.Add(type);
                }
                else
                {
                    notificationTypes.Add(kafkaMsgType, [type]);
                }
            }
        }

        return notificationTypes;
    }

    private void LogHandler(IConsumer<Ignore, JObject?> consumer, LogMessage message) =>
        _logger.Log(message.Level.ToLogLevel(), "{Message}", message.Message);

    private void LogMessage(Guid id, KafkaMessageType type, string topic, int? partition, object? payload)
    {
        var payloadText = JsonConvert.SerializeObject(payload);

        if (payloadText.Length > 1000)
        {
            payloadText = payloadText[..500] + "..." + payloadText[^500..];
        }

        _logger.LogInformation(
            "Message received:\n" +
            "id: {Id}\n" +
            "type: {Type}\n" +
            "topic: {Topic}\n" +
            "partition: {Partition}\n" +
            "payload: {Payload}\n",
            id, type, topic, partition, payloadText);
    }
}