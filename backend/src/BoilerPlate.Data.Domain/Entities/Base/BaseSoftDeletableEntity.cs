using BoilerPlate.Data.Abstractions.Base;

namespace BoilerPlate.Data.Domain.Entities.Base;

/// <summary>
/// Entity with soft deletion
/// </summary>
public abstract class BaseSoftDeletableEntity : BaseAuditableEntity, ISoftDeletable
{
    /// <inheritdoc />
    public Guid? DeletedBy { get; set; }

    /// <inheritdoc />
    public DateTime? DeletedAt { get; set; }

    /// <summary> Entity is soft deleted </summary>
    public bool IsDeleted => DeletedBy.HasValue || DeletedAt.HasValue;
}