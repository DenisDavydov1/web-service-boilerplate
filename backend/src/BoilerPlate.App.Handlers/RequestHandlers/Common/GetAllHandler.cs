using System.Globalization;
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
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BoilerPlate.App.Handlers.RequestHandlers.Common;

public class GetAllHandler<TEntity, TEntityDto>(
    IMapper mapper,
    IUnitOfWork unitOfWork)
    : IRequestHandler<GetAllRequest<TEntity, TEntityDto>, GetAllDto<TEntityDto>>
    where TEntity : BaseEntity
    where TEntityDto : BaseEntityDto
{
    public async Task<GetAllDto<TEntityDto>> Handle(GetAllRequest<TEntity, TEntityDto> request, CancellationToken ct)
    {
        var query = unitOfWork.Repository<TEntity>().GetAllAsQueryable();

        if (string.IsNullOrWhiteSpace(request.Filter) == false)
        {
            query = Filter(query, request.Filter);
        }

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

        var dtos = mapper.Map<IEnumerable<TEntityDto>>(entities);

        return new GetAllDto<TEntityDto>
        {
            Content = dtos,
            Page = page,
            ResultsPerPage = resultsPerPage,
            TotalResults = totalItems,
            IsLast = page * resultsPerPage >= totalItems
        };
    }

    private IQueryable<TEntity> Filter(IQueryable<TEntity> query, string filter)
    {
        var filterRegex = new Regex("([^,:]+:.+?)(?=,[^,:]+:.+|$)");
        var filterFields = filterRegex.Matches(filter)
            .Select(x => x.Value.Split(':', 2))
            .ToDictionary(k => k[0], v => v[1]);

        foreach (var (fieldName, filterValue) in filterFields)
        {
            var parameterExpr = Expression.Parameter(typeof(TEntity));
            var propertyExpr = Expression.Property(parameterExpr, fieldName);

            Expression exprBody;

            if (propertyExpr.Type == typeof(bool))
            {
                var filterBoolValue = bool.Parse(filterValue);
                var filterValueExpr = Expression.Constant(filterBoolValue, typeof(bool));
                exprBody = Expression.Equal(propertyExpr, filterValueExpr);
            }
            else if (propertyExpr.Type == typeof(decimal))
            {
                var filterDecimalValue = decimal.Parse(filterValue);
                var filterValueExpr = Expression.Constant(filterDecimalValue, typeof(decimal));
                exprBody = Expression.Equal(propertyExpr, filterValueExpr);
            }
            else if (propertyExpr.Type == typeof(int))
            {
                var filterIntValue = int.Parse(filterValue);
                var filterValueExpr = Expression.Constant(filterIntValue, typeof(int));
                exprBody = Expression.Equal(propertyExpr, filterValueExpr);
            }
            else if (propertyExpr.Type == typeof(long))
            {
                var filterLongValue = long.Parse(filterValue);
                var filterValueExpr = Expression.Constant(filterLongValue, typeof(long));
                exprBody = Expression.Equal(propertyExpr, filterValueExpr);
            }
            else if (propertyExpr.Type == typeof(float))
            {
                var filterFloatValue = float.Parse(filterValue);
                var filterValueExpr = Expression.Constant(filterFloatValue, typeof(float));
                exprBody = Expression.Equal(propertyExpr, filterValueExpr);
            }
            else if (propertyExpr.Type == typeof(double))
            {
                var filterDoubleValue = double.Parse(filterValue);
                var filterValueExpr = Expression.Constant(filterDoubleValue, typeof(double));
                exprBody = Expression.Equal(propertyExpr, filterValueExpr);
            }
            else if (propertyExpr.Type == typeof(string))
            {
                var methodInfo = typeof(string).GetMethod("Contains", [typeof(string)])!;
                var filterValueExpr = Expression.Constant(filterValue, typeof(string));
                exprBody = Expression.Call(propertyExpr, methodInfo, filterValueExpr);
            }
            else if (propertyExpr.Type.IsEnum)
            {
                var filterEnumValue = Enum.Parse(propertyExpr.Type, filterValue);
                var filterValueExpr = Expression.Constant(filterEnumValue, propertyExpr.Type);
                exprBody = Expression.Equal(propertyExpr, filterValueExpr);
            }
            else if (propertyExpr.Type == typeof(DateTime))
            {
                var filterDateTimeValue = DateTime.Parse(filterValue, CultureInfo.InvariantCulture,
                    DateTimeStyles.RoundtripKind);
                if (filterDateTimeValue.Kind != DateTimeKind.Utc)
                {
                    filterDateTimeValue = filterDateTimeValue.ToUniversalTime();
                }

                var filterValueExpr = Expression.Constant(filterDateTimeValue, typeof(DateTime));
                exprBody = Expression.Equal(propertyExpr, filterValueExpr);
            }
            else if (propertyExpr.Type == typeof(Guid))
            {
                var filterGuidValue = Guid.Parse(filterValue);
                var filterValueExpr = Expression.Constant(filterGuidValue, typeof(Guid));
                exprBody = Expression.Equal(propertyExpr, filterValueExpr);
            }
            else
            {
                continue;
            }

            var lambdaExpr = Expression.Lambda<Func<TEntity, bool>>(exprBody, parameterExpr);
            query = query.Where(lambdaExpr);
        }

        return query;
    }

    private IOrderedQueryable<TEntity> Sort(IQueryable<TEntity> query, string sort)
    {
        var sortRegex = new Regex("([^,:]+:(?:asc|desc))(?=,|$)");
        var sortFields = sortRegex.Matches(sort)
            .Select(x => x.Value.Split(':', 2))
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