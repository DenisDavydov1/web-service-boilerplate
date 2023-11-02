using MediatR;
using BoilerPlate.Data.Domain.Entities.Base;
using BoilerPlate.Data.DTO.Base;

namespace BoilerPlate.Data.DTO.Common.Requests;

/// <summary> Get all entities DTOs request </summary>
public class GetAllRequest<TEntity, TDto> : IRequest<IEnumerable<TDto>>
    where TEntity : BaseEntity
    where TDto : BaseIdDto
{
    /// <summary> Skip N entities </summary>
    public int? Skip { get; set; }

    /// <summary> Take N entities </summary>
    public int? Take { get; set; }
}