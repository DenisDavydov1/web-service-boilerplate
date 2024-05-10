using System.Security.Authentication;
using Microsoft.AspNetCore.Http;
using BoilerPlate.Core.Extensions;
using BoilerPlate.Data.Abstractions.Enums;
using BoilerPlate.Data.DAL.UnitOfWork;
using BoilerPlate.Data.Domain.Entities.System;

namespace BoilerPlate.Services.System.Users;

internal class UsersService(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork)
    : IUsersService
{
    public Guid GetCurrentUserId() =>
        httpContextAccessor.HttpContext?.User.Claims.GetNameIdentifier()
        ?? throw new AuthenticationException();

    public UserRole GetCurrentUserRole() =>
        httpContextAccessor.HttpContext?.User.Claims.GetRole()
        ?? throw new AuthenticationException();

    public async Task<User> GetCurrentUser(CancellationToken ct = default)
    {
        var id = GetCurrentUserId();
        var user = await unitOfWork.Repository<User>().GetByIdAsync(id, ct);
        return user ?? throw new AuthenticationException();
    }
}