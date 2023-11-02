using BoilerPlate.Data.Abstractions.Base;

namespace BoilerPlate.Data.DTO.Base;

/// <summary>
/// DTO with soft deletion
/// </summary>
public abstract class BaseSoftDeletableDto : BaseAuditableDto, ISoftDeletable
{
    /// <inheritdoc />
    public Guid? DeletedBy { get; set; }

    /// <inheritdoc />
    public DateTime? DeletedAt { get; set; }
}