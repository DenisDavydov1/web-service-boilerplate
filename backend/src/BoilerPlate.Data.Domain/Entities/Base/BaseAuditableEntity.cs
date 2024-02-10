using BoilerPlate.Data.Abstractions.Base;

namespace BoilerPlate.Data.Domain.Entities.Base;

/// <summary>
/// Entity with audit fields
/// </summary>
public abstract class BaseAuditableEntity : BaseEntity, IAuditable
{
    /// <inheritdoc />
    public required Guid CreatedBy { get; set; }

    /// <inheritdoc />
    public DateTime CreatedAt { get; set; }

    /// <inheritdoc />
    public Guid? UpdatedBy { get; set; }

    /// <inheritdoc />
    public DateTime? UpdatedAt { get; set; }

    /// <summary> Constructor </summary>
    protected BaseAuditableEntity() => CreatedAt = DateTime.UtcNow;
}