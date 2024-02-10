using System.Security.Authentication;
using Microsoft.AspNetCore.Http;
using BoilerPlate.Core.Extensions;
using BoilerPlate.Data.Abstractions.Enums;
using BoilerPlate.Data.DAL.UnitOfWork;
using BoilerPlate.Data.Domain.Entities.System;

namespace BoilerPlate.Services.System.Users;

internal class UsersService : IUsersService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUnitOfWork _unitOfWork;

    public UsersService(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork)
    {
        _httpContextAccessor = httpContextAccessor;
        _unitOfWork = unitOfWork;
    }

    public Guid GetCurrentUserId() =>
        _httpContextAccessor.HttpContext?.User.Claims.GetNameIdentifier()
        ?? throw new AuthenticationException();

    public UserRole GetCurrentUserRole() =>
        _httpContextAccessor.HttpContext?.User.Claims.GetRole()
        ?? throw new AuthenticationException();

    public async Task<User> GetCurrentUser(CancellationToken ct = default)
    {
        var id = GetCurrentUserId();
        var user = await _unitOfWork.Repository<User>().GetByIdAsync(id, ct);
        return user ?? throw new AuthenticationException();
    }
}