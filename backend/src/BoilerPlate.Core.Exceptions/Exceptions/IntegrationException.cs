namespace BoilerPlate.Core.Exceptions.Exceptions;

/// <summary>
/// External integration error exception
/// </summary>
/// <errorCode> 500 </errorCode>
public class IntegrationException : Exception
{
    public IntegrationException()
    {
    }

    public IntegrationException(string? message = null) : base(message)
    {
    }
}