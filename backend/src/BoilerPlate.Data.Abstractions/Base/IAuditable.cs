namespace BoilerPlate.Data.Abstractions.Base;

/// <summary>
/// Object with audit fields
/// </summary>
public interface IAuditable
{
    /// <summary> User created, ID </summary>
    Guid CreatedBy { get; }

    /// <summary> Creation date </summary>
    DateTime CreatedAt { get; }

    /// <summary> User last updated, ID </summary>
    Guid? UpdatedBy { get; }

    /// <summary> Last update date </summary>
    DateTime? UpdatedAt { get; }
}