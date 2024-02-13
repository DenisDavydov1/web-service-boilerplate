using System.Security.Claims;
using CaseExtensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using BoilerPlate.App.API.Constants;
using BoilerPlate.App.Handlers.Options;
using BoilerPlate.App.Handlers.Utils;
using BoilerPlate.Data.Abstractions.Enums;

namespace BoilerPlate.App.API.Extensions;

/// <summary>
/// Auth settings
/// </summary>
public static class AuthExtensions
{
    /// <summary> Authenticate users with jwt tokens </summary>
    public static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var optionsSection = configuration.GetSection(JwtOptions.SectionName);
        services.Configure<JwtOptions>(optionsSection);
        var options = optionsSection.Get<JwtOptions>();
        if (options == null)
        {
            throw new Exception("JWT options are missing");
        }

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(o =>
            {
                o.IncludeErrorDetails = true;
                o.RequireHttpsMetadata = true;
                o.SaveToken = true;
                o.TokenValidationParameters = JwtUtils.GetTokenValidationParameters(options);
            });
    }

    /// <summary> Authorize users by existing role model </summary>
    public static void AddUserRoleAuthorization(this IServiceCollection services)
    {
        var rolesArray = Enum.GetValues<UserRole>().Select(x => x.ToString().ToSnakeCase()).ToArray();

        services.AddAuthorization(options =>
        {
            options.AddPolicy(AuthorizationConstants.Policy, policyOptions =>
            {
                policyOptions.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                policyOptions.RequireClaim(ClaimTypes.Role, rolesArray);
            });
        });
    }
}