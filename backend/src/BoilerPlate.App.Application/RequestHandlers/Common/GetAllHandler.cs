using System.ComponentModel;
using AutoMapper;
using BoilerPlate.Core.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using BoilerPlate.Data.DAL.UnitOfWork;
using BoilerPlate.Data.Domain.Entities.Base;
using BoilerPlate.Data.DTO.Base;
using BoilerPlate.Data.DTO.Common.Requests;
using BoilerPlate.Data.DTO.Common.Responses;

namespace BoilerPlate.App.Application.RequestHandlers.Common;

public class GetAllHandler<TEntity, TEntityDto> : IRequestHandler<GetAllRequest<TEntity, TEntityDto>, GetAllDto<TEntityDto>>
    where TEntity : BaseEntity
    where TEntityDto : BaseEntityDto
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<GetAllDto<TEntityDto>> Handle(GetAllRequest<TEntity, TEntityDto> request, CancellationToken ct)
    {
        var query = _unitOfWork.Repository<TEntity>()
            .GetAllAsQueryable()
            .WhereIf(request.Filter != null, request.Filter!);

        var totalItems = query.Count();

        if (request.Sort != null)
        {
            query = request.SortDirection == ListSortDirection.Descending
                ? query.OrderByDescending(request.Sort)
                : query.OrderBy(request.Sort);
        }

        var page = request.Page ?? 1;
        var pageSize = request.PageSize ?? 100;
        var entities = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        var dtos = _mapper.Map<IEnumerable<TEntityDto>>(entities);

        return new GetAllDto<TEntityDto>
        {
            Items = dtos,
            Page = page,
            PageSize = pageSize,
            TotalItems = totalItems,
            HasMore = page * pageSize < totalItems
        };
    }
}