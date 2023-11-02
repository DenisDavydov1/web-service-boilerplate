using BoilerPlate.Data.Abstractions.Base;

namespace BoilerPlate.Data.Domain.Entities.Base;

/// <summary>
/// Entity with ID
/// </summary>
public abstract class BaseIdEntity : BaseEntity, IIdentifiable
{
    /// <inheritdoc />
    public Guid Id { get; set; }
}