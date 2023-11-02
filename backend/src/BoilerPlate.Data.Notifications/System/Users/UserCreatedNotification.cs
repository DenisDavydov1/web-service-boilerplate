using MediatR;
using BoilerPlate.Data.Abstractions.System;

namespace BoilerPlate.Data.Notifications.System.Users;

public class UserCreatedNotification : INotification
{
    public IUser User { get; set; } = null!;
}