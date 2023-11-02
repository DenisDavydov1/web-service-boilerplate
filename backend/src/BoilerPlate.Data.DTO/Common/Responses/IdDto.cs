using BoilerPlate.Data.Abstractions.Base;

namespace BoilerPlate.Data.DTO.Common.Responses;

/// <summary>
/// Object with ID DTO
/// </summary>
public class IdDto : IIdentifiable
{
    /// <inheritdoc />
    public required Guid Id { get; set; }
}