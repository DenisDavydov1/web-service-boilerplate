using System.Diagnostics.CodeAnalysis;
using BoilerPlate.Data.Domain.Entities.Base;
using BoilerPlate.Data.DTO.Base;
using BoilerPlate.Data.DTO.Common.Responses;
using MediatR;

namespace BoilerPlate.Data.DTO.Common.Requests;

/// <summary> Get all entities DTOs request </summary>
[SuppressMessage("ReSharper", "UnusedTypeParameter")]
public class GetAllRequest<TEntity, TEntityDto> : IRequest<GetAllDto<TEntityDto>>
    where TEntity : BaseEntity
    where TEntityDto : BaseEntityDto
{
    /// <summary> Page number </summary>
    public int? Page { get; set; }

    /// <summary> Page size </summary>
    public int? ResultsPerPage { get; set; }

    /// <summary> Query filter string </summary>
    /// <example> firstName:Ivan,age:22 </example>
    public string? Filter { get; set; }

    /// <summary> Sort string </summary>
    /// <example> firstName:asc,age:desc </example>
    public string? Sort { get; set; }
}