using BoilerPlate.Core.Exceptions.Enums;

namespace BoilerPlate.Core.Exceptions.Factory;

public interface IExceptionFactory
{
    void ThrowIf<TException>(bool condition, ExceptionCode exceptionCode, params object?[] args)
        where TException : Exception;

    void Throw<TException>(ExceptionCode exceptionCode, params object?[] args)
        where TException : Exception;

    TException Get<TException>(ExceptionCode exceptionCode, params object?[] args)
        where TException : Exception;
}