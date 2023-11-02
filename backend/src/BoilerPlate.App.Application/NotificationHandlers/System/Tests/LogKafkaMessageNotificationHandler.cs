using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using BoilerPlate.Core.Serialization;
using BoilerPlate.Data.Notifications.System.Tests;
using BoilerPlate.Services.Kafka.Constants;

namespace BoilerPlate.App.Application.NotificationHandlers.System.Tests;

public class LogKafkaMessageNotificationHandler : INotificationHandler<LogKafkaMessageNotification>
{
    private readonly ILogger<LogKafkaMessageNotificationHandler> _logger;

    public LogKafkaMessageNotificationHandler(ILogger<LogKafkaMessageNotificationHandler> logger) => _logger = logger;

    public Task Handle(LogKafkaMessageNotification notification, CancellationToken ct)
    {
        var messageValue = JsonConvert.SerializeObject(notification.Payload, SerializationSettings.Default);
        if (messageValue.Length > 1000)
        {
            messageValue = messageValue[..500] + "..." + messageValue[^500..];
        }

        _logger.LogInformation(
            "Kafka consumer received message:\n" +
            "{MessageIdHeader}: {MessageId}\n" +
            "{MessageTypeHeader}: {MessageType}\n" +
            "value: {MessageValue}\n",
            MessageHeaders.MessageId, notification.MessageId,
            MessageHeaders.MessageType, notification.MessageType,
            messageValue);

        return Task.CompletedTask;
    }
}