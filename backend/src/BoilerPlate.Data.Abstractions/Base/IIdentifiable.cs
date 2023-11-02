namespace BoilerPlate.Data.Abstractions.Base;

/// <summary>
/// Object with ID
/// </summary>
public interface IIdentifiable
{
    /// <summary> ID </summary>
    public Guid Id { get; }
}