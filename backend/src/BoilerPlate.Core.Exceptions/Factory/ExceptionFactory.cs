using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using BoilerPlate.Core.Constants;
using BoilerPlate.Core.Exceptions.Enums;
using BoilerPlate.Core.Extensions;

namespace BoilerPlate.Core.Exceptions.Factory;

internal class ExceptionFactory : IExceptionFactory
{
    private readonly string _languageCode;

    public ExceptionFactory(IHttpContextAccessor httpContextAccessor) =>
        _languageCode = httpContextAccessor.HttpContext?.User.FindFirstValue(LanguageCodes.ClaimLanguageCode)
                        ?? LanguageCodes.English;

    public void ThrowIf<TException>(bool condition, ExceptionCode exceptionCode, params object?[] args)
        where TException : Exception, new()
    {
        if (condition)
        {
            Throw<TException>(exceptionCode, args);
        }
    }

    public void Throw<TException>(ExceptionCode exceptionCode, params object?[] args)
        where TException : Exception, new()
        => throw Get<TException>(exceptionCode, args);

    public TException Get<TException>(ExceptionCode exceptionCode, params object?[] args)
        where TException : Exception, new()
    {
        var exceptionText = exceptionCode.GetText(_languageCode);
        var argsWithText = args.Prepend(exceptionText).ToArray();

        var exception = (TException?) Activator.CreateInstance(typeof(TException), argsWithText);
        if (exception == null)
        {
            throw new NullReferenceException("Invalid exception type or parameters");
        }

        return exception;
    }
}