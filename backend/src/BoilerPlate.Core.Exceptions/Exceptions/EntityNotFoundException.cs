using BoilerPlate.Core.Exceptions.Enums;

namespace BoilerPlate.Core.Exceptions.Exceptions;

public class EntityNotFoundException(ExceptionCode code, string message, string? paramName)
    : BaseExceptionWithCode(code, message)
{
    public string? ParamName { get; set; } = paramName;
}