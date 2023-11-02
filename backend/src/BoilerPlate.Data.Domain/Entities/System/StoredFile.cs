using BoilerPlate.Data.Abstractions.System;
using BoilerPlate.Data.Domain.Entities.Base;

namespace BoilerPlate.Data.Domain.Entities.System;

/// <summary>
/// <inheritdoc cref="IStoredFile"/> entity
/// </summary>
public class StoredFile : BaseAuditableEntity, IStoredFile
{
    /// <inheritdoc />
    public required string Name { get; set; } = null!;
}