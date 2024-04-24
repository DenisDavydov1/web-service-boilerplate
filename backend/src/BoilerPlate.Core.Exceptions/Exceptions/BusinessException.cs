using BoilerPlate.Core.Exceptions.Enums;

namespace BoilerPlate.Core.Exceptions.Exceptions;

/// <summary>
/// Business logics error exception
/// </summary>
/// <errorCode> 400 </errorCode>
public class BusinessException(ExceptionCode code, string message, params string[]? parameterNames)
    : BaseExceptionWithCode(code, message)
{
    public IEnumerable<string>? ParameterNames { get; } = parameterNames;
}