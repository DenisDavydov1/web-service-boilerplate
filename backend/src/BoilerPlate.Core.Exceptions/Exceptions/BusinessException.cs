namespace BoilerPlate.Core.Exceptions.Exceptions;

/// <summary>
/// Business logics error exception
/// </summary>
/// <errorCode> 400 </errorCode>
public class BusinessException : Exception
{
    public IEnumerable<string>? ParameterNames { get; }

    public BusinessException()
    {
    }

    public BusinessException(string message) : base(message)
    {
    }

    public BusinessException(string message, params string[]? parameterNames) : base(message) =>
        ParameterNames = parameterNames;
}