using BoilerPlate.Data.Abstractions.Base;

namespace BoilerPlate.Data.DTO.Base;

/// <summary>
/// DTO with ID
/// </summary>
public abstract class BaseIdDto : BaseDto, IIdentifiable
{
    /// <inheritdoc />
    public required Guid Id { get; set; }
}