using MediatR;
using Newtonsoft.Json;
using BoilerPlate.Data.Abstractions.System;
using BoilerPlate.Data.DTO.Base;
using BoilerPlate.Data.DTO.Common.Responses;

namespace BoilerPlate.Data.DTO.System.StoredFiles.Requests;

/// <summary>
/// Update stored file request DTO
/// </summary>
public class UpdateStoredFileDto : BaseDto, IRequest<IdDto>, IStoredFile
{
    /// <summary> Stored file ID </summary>
    [JsonIgnore]
    public Guid Id { get; set; }

    /// <inheritdoc />
    public required string Name { get; init; }
}