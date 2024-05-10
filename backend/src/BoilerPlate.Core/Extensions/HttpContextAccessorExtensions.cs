using System.Security.Claims;
using BoilerPlate.Core.Constants;
using Microsoft.AspNetCore.Http;

namespace BoilerPlate.Core.Extensions;

public static class HttpContextAccessorExtensions
{
    public static string GetUserLanguageCode(this IHttpContextAccessor httpContextAccessor) =>
        httpContextAccessor.HttpContext?.User.FindFirstValue(LanguageCodes.ClaimLanguageCode)
        ?? LanguageCodes.English;
}