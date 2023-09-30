using System.Linq.Expressions;
using ExchangeRate.Aggregator.Shared.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ExchangeRate.Aggregator.Shared.Infrastructure.Repositories
{
    /// <summary>
    /// Methods that let generically include data in EF
    /// </summary>
    public static class IncludableExtensions
    {
        public static IQueryable<TSource> Include<TSource>(this IQueryable<TSource> queryable, params string[] navigations)
            where TSource : class
        {
            if (navigations == null || navigations.Length == 0)
            {
                return queryable;
            }

            return navigations.Aggregate(queryable, EntityFrameworkQueryableExtensions.Include);
        }

        public static IIncludable<TEntity, TProperty> Include<TEntity, TProperty>(
            this IIncludable<TEntity> includes,
            Expression<Func<TEntity, TProperty>> propertySelector)
            where TEntity : class
        {
            var result = ((Includables.Includable<TEntity>)includes).Input
                .Include(propertySelector);
            return new Includables.Includable<TEntity, TProperty>(result);
        }

        public static IIncludable<TEntity, TOtherProperty> ThenInclude<TEntity, TOtherProperty, TProperty>(
            this IIncludable<TEntity, TProperty> includes,
            Expression<Func<TProperty, TOtherProperty>> propertySelector)
            where TEntity : class
        {
            var result = ((Includables.Includable<TEntity, TProperty>)includes)
                .IncludableInput.ThenInclude(propertySelector);

            return new Includables.Includable<TEntity, TOtherProperty>(result);
        }

        public static IIncludable<TEntity, TOtherProperty> ThenInclude<TEntity, TOtherProperty, TProperty>(
            this IIncludable<TEntity,
            ICollection<TProperty>> includes,
            Expression<Func<TProperty, TOtherProperty>> propertySelector)
            where TEntity : class
        {
            var result = ((Includables.Includable<TEntity, ICollection<TProperty>>)includes)
                .IncludableInput.ThenInclude(propertySelector);

            return new Includables.Includable<TEntity, TOtherProperty>(result);
        }

        public static IIncludable<TEntity, TOtherProperty> ThenInclude<TEntity, TOtherProperty, TProperty>(
            this IIncludable<TEntity,
            IEnumerable<TProperty>> includes,
            Expression<Func<TProperty, TOtherProperty>> propertySelector)
            where TEntity : class
        {
            var result = ((Includables.Includable<TEntity, IEnumerable<TProperty>>)includes)
                .IncludableInput.ThenInclude(propertySelector);

            return new Includables.Includable<TEntity, TOtherProperty>(result);
        }

        public static IQueryable<T> IncludeMultiple<T>(this IQueryable<T> query, Func<IIncludable<T>, IIncludable> includes)
            where T : class
        {
            if (includes == null)
            {
                return query;
            }

            var includable = (Includables.Includable<T>)includes(new Includables.Includable<T>(query));

            return includable.Input;
        }

        public static IQueryable<T> IncludeMultiple<T>(this IQueryable<T> query, params string[] includes)
            where T : class
        {
            if (includes == null)
            {
                return query;
            }

            query = includes.Aggregate(query, (current, include) => current.Include(include));

            return query;
        }
    }
}
