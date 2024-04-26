using System.Security.Claims;
using System.Text.RegularExpressions;
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

    public void ThrowIf<TException>(bool condition, ExceptionCode exceptionCode,
        object?[]? args = null, object?[]? formatArgs = null)
        where TException : Exception
    {
        if (condition)
        {
            Throw<TException>(exceptionCode, args, formatArgs);
        }
    }

    public void Throw<TException>(ExceptionCode exceptionCode, object?[]? args = null, object?[]? formatArgs = null)
        where TException : Exception =>
        throw Get<TException>(exceptionCode, args, formatArgs);

    public TException Get<TException>(ExceptionCode exceptionCode, object?[]? args = null, object?[]? formatArgs = null)
        where TException : Exception
    {
        args ??= [];
        var formatArguments = new List<object?>(formatArgs ?? []);

        var exceptionTextTemplate = exceptionCode.GetText(_languageCode);
        var templateVariablesCountRegex = new Regex("({[^{}]+})(?=[^}])");
        var variablesCount = templateVariablesCountRegex.Matches(exceptionTextTemplate).Count;
        formatArguments.AddRange(Enumerable.Repeat<object?>(null, variablesCount - formatArguments.Count));
        var exceptionText = string.Format(exceptionTextTemplate, formatArguments.ToArray());
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