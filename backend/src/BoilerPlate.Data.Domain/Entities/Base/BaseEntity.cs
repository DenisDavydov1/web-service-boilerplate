using BoilerPlate.Data.Abstractions.Base;

namespace BoilerPlate.Data.Domain.Entities.Base;

/// <summary>
/// Entity
/// </summary>
public abstract class BaseEntity : IIdentifiable
{
    /// <inheritdoc />
    public Guid Id { get; set; }
}