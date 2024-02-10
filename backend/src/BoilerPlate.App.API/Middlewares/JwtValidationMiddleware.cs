using Microsoft.AspNetCore.Authorization;
using BoilerPlate.Core.Extensions;
using BoilerPlate.Data.DAL.UnitOfWork;
using BoilerPlate.Data.Domain.Entities.System;

namespace BoilerPlate.App.API.Middlewares;

/// <summary>
/// Validate jwt
/// </summary>
public class JwtValidationMiddleware : IMiddleware
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary> Constructor </summary>
    public JwtValidationMiddleware(IUnitOfWork unitOfWork) =>
        _unitOfWork = unitOfWork;

    /// <inheritdoc />
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var allowAnonymousAttribute = context.GetEndpoint()?.Metadata.GetMetadata<AllowAnonymousAttribute>();
        if (allowAnonymousAttribute == null)
        {
            var userId = context.User.Claims.GetNameIdentifier();
            var jti = context.User.Claims.GetJti();
            var exp = context.User.Claims.GetExp();

            if (userId == null || jti == null || exp == null)
            {
                context.Response.StatusCode = 401;
                return;
            }

            var user = await _unitOfWork.Repository<User>().GetByIdAsync(userId.Value);
            if (user == null || user.IsDeleted || user.AccessTokenId != jti || user.AccessTokenExpiresAt != exp)
            {
                context.Response.StatusCode = 401;
                return;
            }
        }

        await next(context);
    }
}