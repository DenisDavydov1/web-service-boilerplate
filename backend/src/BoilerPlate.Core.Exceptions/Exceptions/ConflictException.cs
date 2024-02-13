namespace BoilerPlate.Core.Exceptions.Exceptions;

/// <summary>
/// Conflict error exception
/// </summary>
/// <errorCode> 409 </errorCode>
public class ConflictException : Exception
{
    public ConflictException()
    {
    }

    public ConflictException(string message) : base(message)
    {
    }
}