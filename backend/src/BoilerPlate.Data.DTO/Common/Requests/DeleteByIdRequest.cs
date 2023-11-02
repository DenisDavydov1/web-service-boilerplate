using MediatR;
using BoilerPlate.Data.Domain.Entities.Base;
using BoilerPlate.Data.DTO.Common.Responses;

namespace BoilerPlate.Data.DTO.Common.Requests;

/// <summary> Delete entity by ID </summary>
public class DeleteByIdRequest<TEntity> : IRequest<IdDto>
    where TEntity : BaseIdEntity
{
    /// <summary> Entity ID </summary>
    public required Guid Id { get; init; }
}