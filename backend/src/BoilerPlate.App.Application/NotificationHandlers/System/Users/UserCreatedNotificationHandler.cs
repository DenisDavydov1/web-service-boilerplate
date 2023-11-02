using MediatR;
using BoilerPlate.Data.Notifications.System.Users;

namespace BoilerPlate.App.Application.NotificationHandlers.System.Users;

public class UserCreatedNotificationHandler : INotificationHandler<UserCreatedNotification>
{
    public Task Handle(UserCreatedNotification notification, CancellationToken ct) => Task.CompletedTask;
}