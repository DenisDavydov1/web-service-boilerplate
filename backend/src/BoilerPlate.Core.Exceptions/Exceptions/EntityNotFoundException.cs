namespace BoilerPlate.Core.Exceptions.Exceptions;

public class EntityNotFoundException : Exception
{
    public string? ParamName { get; set; }

    public EntityNotFoundException() { }

    public EntityNotFoundException(string message) : base(message) { }

    public EntityNotFoundException(string message, string? paramName) : base(message) => ParamName = paramName;
}