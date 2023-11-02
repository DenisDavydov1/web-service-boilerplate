namespace BoilerPlate.Core.Extensions;

public static class EnumerableExtensions
{
    public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> enumerable,
        bool condition, Func<TSource, bool> predicate) =>
        condition ? enumerable.Where(predicate) : enumerable;
}