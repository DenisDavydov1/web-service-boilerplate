using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using BoilerPlate.Core.Constants;
using BoilerPlate.Core.Exceptions.Enums;
using BoilerPlate.Core.Exceptions.Exceptions;
using BoilerPlate.Core.Extensions;

namespace BoilerPlate.Core.Exceptions.Factory;

internal class ExceptionFactory(IHttpContextAccessor httpContextAccessor) : IExceptionFactory
{
    private readonly string _languageCode =
        httpContextAccessor.HttpContext?.User.FindFirstValue(LanguageCodes.ClaimLanguageCode)
        ?? LanguageCodes.English;

    public void ThrowIf<TException>(bool condition, ExceptionCode exceptionCode, params object?[] args)
        where TException : Exception
    {
        if (condition)
        {
            Throw<TException>(exceptionCode, args);
        }
    }

    public void Throw<TException>(ExceptionCode exceptionCode, params object?[] args)
        where TException : Exception
        => throw Get<TException>(exceptionCode, args);

    public TException Get<TException>(ExceptionCode exceptionCode, params object?[] args)
        where TException : Exception
    {
        var exceptionText = exceptionCode.GetText(_languageCode);
        var argsWithText = args.Prepend(exceptionText).ToArray();

        TException? exception;

        if (typeof(TException).IsAssignableTo(typeof(BaseExceptionWithCode)))
        {
            var argsWithCode = argsWithText.Prepend(exceptionCode).ToArray();
            exception = (TException?) Activator.CreateInstance(typeof(TException), argsWithCode);
        }
        else
        {
            exception = (TException?) Activator.CreateInstance(typeof(TException), argsWithText);
        }

        if (exception == null)
        {
            throw new NullReferenceException("Invalid exception type or parameters");
        }

        return exception;
    }
}