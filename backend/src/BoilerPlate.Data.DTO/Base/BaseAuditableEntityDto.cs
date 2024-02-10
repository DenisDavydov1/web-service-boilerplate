using BoilerPlate.Data.Abstractions.Base;

namespace BoilerPlate.Data.DTO.Base;

/// <summary>
/// DTO with audit fields
/// </summary>
public abstract class BaseAuditableEntityDto : BaseEntityDto, IAuditable
{
    /// <inheritdoc />
    public required Guid CreatedBy { get; set; }

    /// <inheritdoc />
    public required DateTime CreatedAt { get; set; }

    /// <inheritdoc />
    public Guid? UpdatedBy { get; set; }

    /// <inheritdoc />
    public DateTime? UpdatedAt { get; set; }
}