using BoilerPlate.Core.Exceptions.Enums;

namespace BoilerPlate.Core.Exceptions.Exceptions;

public abstract class BaseExceptionWithCode(ExceptionCode code, string message) : Exception(message)
{
    public ExceptionCode Code { get; set; } = code;
}