using BoilerPlate.Core.Exceptions.Enums;

namespace BoilerPlate.Core.Exceptions.Factory;

public interface IExceptionFactory
{
    void ThrowIf<TException>(bool condition, ExceptionCode exceptionCode,
        object?[]? args = null, object?[]? formatArgs = null)
        where TException : Exception;

    void Throw<TException>(ExceptionCode exceptionCode, object?[]? args = null, object?[]? formatArgs = null)
        where TException : Exception;

    TException Get<TException>(ExceptionCode exceptionCode, object?[]? args = null, object?[]? formatArgs = null)
        where TException : Exception;
}