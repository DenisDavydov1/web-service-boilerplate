using BoilerPlate.Data.Abstractions.System;
using BoilerPlate.Data.DTO.Base;

namespace BoilerPlate.Data.DTO.System.StoredFiles.Responses;

/// <summary>
/// Stored file DTO
/// </summary>
public sealed class StoredFileDto : BaseAuditableEntityDto, IStoredFile
{
    /// <inheritdoc />
    public required string Name { get; set; }
}