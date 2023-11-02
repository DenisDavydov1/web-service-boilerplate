namespace BoilerPlate.Core.Exceptions.Exceptions;

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