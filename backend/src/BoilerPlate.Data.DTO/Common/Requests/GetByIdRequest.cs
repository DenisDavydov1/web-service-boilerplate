using System.Diagnostics.CodeAnalysis;
using MediatR;
using BoilerPlate.Data.Domain.Entities.Base;
using BoilerPlate.Data.DTO.Base;

namespace BoilerPlate.Data.DTO.Common.Requests;

/// <summary> Get entity DTO by ID </summary>
[SuppressMessage("ReSharper", "UnusedTypeParameter")]
public class GetByIdRequest<TEntity, TDto> : IRequest<TDto>
    where TEntity : BaseEntity
    where TDto : BaseEntityDto
{
    /// <summary> Entity ID </summary>
    public required Guid Id { get; init; }
}