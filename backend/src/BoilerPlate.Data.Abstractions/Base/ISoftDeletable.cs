namespace BoilerPlate.Data.Abstractions.Base;

/// <summary>
/// Object with soft deletion
/// </summary>
public interface ISoftDeletable
{
    /// <summary> User soft deleted, ID </summary>
    Guid? DeletedBy { get; }

    /// <summary> Deletion date </summary>
    DateTime? DeletedAt { get; }
}