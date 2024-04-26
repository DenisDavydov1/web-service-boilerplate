using System.Linq.Expressions;

namespace BoilerPlate.Core.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> WhereIf<T>(this IQueryable<T> queryable,
        bool condition, Expression<Func<T,bool>> predicate) =>
        condition ? queryable.Where(predicate) : queryable;

    public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName) =>
        source.OrderBy(ToLambda<T>(propertyName));

    public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string propertyName) =>
        source.OrderByDescending(ToLambda<T>(propertyName));

    public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string propertyName) =>
        source.ThenBy(ToLambda<T>(propertyName));

    public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> source, string propertyName) =>
        source.ThenByDescending(ToLambda<T>(propertyName));

    private static Expression<Func<T, object>> ToLambda<T>(string propertyName)
    {
        var parameter = Expression.Parameter(typeof(T));
        var property = Expression.Property(parameter, propertyName);
        var propertyAsObject = Expression.Convert(property, typeof(object));

        return Expression.Lambda<Func<T, object>>(propertyAsObject, parameter);
    }


}