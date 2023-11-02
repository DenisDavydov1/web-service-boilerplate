using CaseExtensions;
using Microsoft.AspNetCore.Authorization;
using BoilerPlate.Data.Abstractions.Enums;

namespace BoilerPlate.App.API.Attributes;

/// <summary>
/// Validate role claim in auth
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class MinimumRoleAuthorizeAttribute : AuthorizeAttribute
{
    /// <inheritdoc />
    public MinimumRoleAuthorizeAttribute(UserRole minimumRequiredRole)
    {
        var allowedRoles = Enum.GetValues<UserRole>().Where(x => x >= minimumRequiredRole);
        Roles = string.Join(',', allowedRoles.Select(x => x.ToString().ToSnakeCase()));
    }
}