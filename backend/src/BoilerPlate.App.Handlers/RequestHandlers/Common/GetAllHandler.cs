using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using AutoMapper;
using BoilerPlate.Core.Exceptions.Enums;
using BoilerPlate.Core.Exceptions.Exceptions;
using BoilerPlate.Core.Exceptions.Factory;
using BoilerPlate.Core.Extensions;
using BoilerPlate.Data.DAL.UnitOfWork;
using BoilerPlate.Data.Domain.Entities.Base;
using BoilerPlate.Data.DTO.Base;
using BoilerPlate.Data.DTO.Common.Requests;
using BoilerPlate.Data.DTO.Common.Responses;
using CaseExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BoilerPlate.App.Handlers.RequestHandlers.Common;

public partial class GetAllHandler<TEntity, TEntityDto> : IRequestHandler<GetAllRequest<TEntity, TEntityDto>, GetAllDto<TEntityDto>>
    where TEntity : BaseEntity
    where TEntityDto : BaseEntityDto
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IExceptionFactory _exceptionFactory;

    public GetAllHandler(IMapper mapper, IUnitOfWork unitOfWork, IExceptionFactory exceptionFactory)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _exceptionFactory = exceptionFactory;
    }

    public async Task<GetAllDto<TEntityDto>> Handle(GetAllRequest<TEntity, TEntityDto> request, CancellationToken ct)
    {
        var query = _unitOfWork.Repository<TEntity>()
            .GetAllAsQueryable()
            .WhereIf(request.Filter != null, request.Filter!);

        var totalItems = query.Count();

        if (string.IsNullOrWhiteSpace(request.Sort) == false)
        {
            query = Sort(query, request.Sort);
        }

        var page = request.Page ?? 1;
        var resultsPerPage = request.ResultsPerPage ?? 100;
        var entities = await query
            .Skip((page - 1) * resultsPerPage)
            .Take(resultsPerPage)
            .ToListAsync(ct);

        var dtos = _mapper.Map<IEnumerable<TEntityDto>>(entities);

        return new GetAllDto<TEntityDto>
        {
            Content = dtos,
            Page = page,
            ResultsPerPage = resultsPerPage,
            TotalResults = totalItems,
            IsLast = page * resultsPerPage >= totalItems
        };
    }

    [GeneratedRegex(@"(.+:(asc|desc))(\,|$)\b")]
    private static partial Regex SortRegex();

    private IOrderedQueryable<TEntity> Sort(IQueryable<TEntity> query, string sort)
    {
        var sortRegex = SortRegex();
        _exceptionFactory.ThrowIf<BusinessException>(
            sortRegex.IsMatch(sort) == false,
            ExceptionCode.Common_GetAll_InvalidSortString);

        var sortFields = sort.Split(',')
            .Select(x => x.Split(':'))
            .ToDictionary(k => k[0], v => v[1]);

        var orderedQueryable = query.OrderBy(_ => 0);

        foreach (var (fieldName, sortDirection) in sortFields)
        {
            orderedQueryable = sortDirection switch
            {
                "asc" => orderedQueryable.ThenBy(fieldName),
                "desc" => orderedQueryable.ThenByDescending(fieldName),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        return orderedQueryable;
    }
}