using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EnergySector.LightSchedule.DataAccess.EntityFrameworkCore;

public static class QueryableExtensions
{
    public static IQueryable<TSource> WhereIf<TSource>(
        this IQueryable<TSource> source,
        bool condition,
        Expression<Func<TSource, bool>> predicate)
    {
        if (condition)
        {
            return source.Where(predicate);
        }

        return source;
    }

    public static IQueryable<TEntity> IncludeIf<TEntity, TProperty>(
        this IQueryable<TEntity> source,
        bool condition,
        Expression<Func<TEntity, TProperty>> navigationPropertyPath) where TEntity : class
    {
        if (condition)
        {
            return source.Include(navigationPropertyPath);
        }

        return source;
    }
}
