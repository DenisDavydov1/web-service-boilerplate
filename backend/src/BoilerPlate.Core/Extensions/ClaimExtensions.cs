using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BoilerPlate.Data.Abstractions.Enums;

namespace BoilerPlate.Core.Extensions;

public static class ClaimExtensions
{
    public static Guid? GetNameIdentifier(this IEnumerable<Claim> claims)
    {
        var claim = claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
        return claim != null ? Guid.Parse(claim.Value) : null;
    }

    public static Guid? GetJti(this IEnumerable<Claim> claims)
    {
        var claim = claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti);
        return claim != null ? Guid.Parse(claim.Value) : null;
    }

    public static DateTime? GetExp(this IEnumerable<Claim> claims)
    {
        var claim = claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp);
        if (claim == null) return null;

        var epoch = int.Parse(claim.Value);
        var dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(epoch);
        return dateTimeOffset.UtcDateTime;
    }

    public static UserRole? GetRole(this IEnumerable<Claim> claims)
    {
        var claim = claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);
        return claim != null ? Enum.Parse<UserRole>(claim.Value, ignoreCase: true) : null;
    }
}