using System.Linq.Expressions;

namespace BoilerPlate.Core.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> queryable,
        bool condition, Expression<Func<TSource,bool>> predicate) =>
        condition ? queryable.Where(predicate) : queryable;
}