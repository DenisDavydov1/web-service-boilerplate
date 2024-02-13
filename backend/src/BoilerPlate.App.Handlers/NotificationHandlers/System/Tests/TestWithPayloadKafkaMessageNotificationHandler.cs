using MediatR;
using Microsoft.Extensions.Logging;
using BoilerPlate.Data.Notifications.System.Tests;

namespace BoilerPlate.App.Handlers.NotificationHandlers.System.Tests;

public class TestWithPayloadKafkaMessageNotificationHandler
    : INotificationHandler<TestWithPayloadKafkaMessageNotification>
{
    private readonly ILogger<TestWithPayloadKafkaMessageNotificationHandler> _logger;

    public TestWithPayloadKafkaMessageNotificationHandler(
        ILogger<TestWithPayloadKafkaMessageNotificationHandler> logger) => _logger = logger;

    public Task Handle(TestWithPayloadKafkaMessageNotification notification, CancellationToken ct)
    {
        _logger.LogInformation("Test with payload. Property value: {Property}", notification.Payload?.Property);
        return Task.CompletedTask;
    }
}