using BoilerPlate.Data.Abstractions.Enums;
using BoilerPlate.Data.Domain.Entities.System;

namespace BoilerPlate.Services.System.Users;

public interface IUsersService
{
    Guid GetCurrentUserId();
    UserRole GetCurrentUserRole();
    Task<User> GetCurrentUser(CancellationToken ct = default);
}