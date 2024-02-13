using MediatR;
using Microsoft.Extensions.Logging;
using BoilerPlate.Data.Notifications.System.Tests;

namespace BoilerPlate.App.Handlers.NotificationHandlers.System.Tests;

public class TestKafkaMessageNotificationHandler : INotificationHandler<TestKafkaMessageNotification>
{
    private readonly ILogger<TestKafkaMessageNotificationHandler> _logger;

    public TestKafkaMessageNotificationHandler(ILogger<TestKafkaMessageNotificationHandler> logger) => _logger = logger;

    public Task Handle(TestKafkaMessageNotification notification, CancellationToken ct)
    {
        _logger.LogInformation("Test without payload");
        return Task.CompletedTask;
    }
}