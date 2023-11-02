using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using CaseExtensions;
using Microsoft.IdentityModel.Tokens;
using BoilerPlate.App.Application.Options;
using BoilerPlate.Core.Constants;
using BoilerPlate.Data.Domain.Entities.System;

namespace BoilerPlate.App.Application.Utils;

public static class JwtUtils
{
    public static TokenValidationParameters GetTokenValidationParameters(JwtOptions options) =>
        new()
        {
            ValidateIssuer = true,
            ValidIssuer = options.Issuer,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = GetSecurityKey(options.PublicKey),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

    public static JwtSecurityToken GenerateAccessToken(JwtOptions jwtOptions, User user) =>
        GenerateToken(
            privateKey: jwtOptions.PrivateKey,
            issuer: jwtOptions.Issuer,
            lifeTimeInMinutes: jwtOptions.LifetimeInMinutes,
            claims: new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(LanguageCodes.ClaimLanguageCode, user.LanguageCode),
                new Claim(ClaimTypes.Role, user.Role.ToString().ToSnakeCase())
            });

    public static JwtSecurityToken GenerateRefreshToken(JwtOptions jwtOptions, User user) =>
        GenerateToken(
            privateKey: jwtOptions.PrivateKey,
            issuer: jwtOptions.Issuer,
            lifeTimeInMinutes: jwtOptions.RefreshTokenLifetimeInMinutes,
            claims: new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            });

    public static string WriteToken(JwtSecurityToken token) =>
        new JwtSecurityTokenHandler().WriteToken(token);

    private static JwtSecurityToken GenerateToken(string privateKey, string issuer, int lifeTimeInMinutes,
        IEnumerable<Claim> claims)
    {
        var credentials = new SigningCredentials(GetSecurityKey(privateKey), SecurityAlgorithms.RsaSha256);

        claims = claims
            .Where(x => x.Type != JwtRegisteredClaimNames.Jti)
            .Append(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

        var token = new JwtSecurityToken(
            issuer: issuer,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(lifeTimeInMinutes),
            signingCredentials: credentials);

        return token;
    }

    private static RsaSecurityKey GetSecurityKey(string pemKey)
    {
        var rsa = RSA.Create(2048);
        rsa.ImportFromPem(pemKey);
        return new RsaSecurityKey(rsa);
    }
}