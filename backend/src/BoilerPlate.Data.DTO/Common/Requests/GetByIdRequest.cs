using MediatR;
using BoilerPlate.Data.Domain.Entities.Base;
using BoilerPlate.Data.DTO.Base;

namespace BoilerPlate.Data.DTO.Common.Requests;

/// <summary> Get entity DTO by ID </summary>
public class GetByIdRequest<TEntity, TDto> : IRequest<TDto>
    where TEntity : BaseIdEntity
    where TDto : BaseIdDto
{
    /// <summary> Entity ID </summary>
    public required Guid Id { get; init; }
}