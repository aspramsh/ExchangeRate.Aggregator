using Microsoft.EntityFrameworkCore.Query;

namespace ExchangeRate.Aggregator.Shared.Infrastructure.Repositories
{
    /// <summary>
    /// Table interface implementation to be included
    /// </summary>
    internal class Includables
    {
        internal class Includable<TEntity> : IIncludable<TEntity>
            where TEntity : class
        {
            internal Includable(IQueryable<TEntity> queryable) => Input = queryable
                                                                          ?? throw new ArgumentNullException(
                                                                              nameof(queryable));

            internal IQueryable<TEntity> Input { get; }
        }

        internal class Includable<TEntity, TProperty> : Includable<TEntity>, IIncludable<TEntity, TProperty>
            where TEntity : class
        {
            internal Includable(IIncludableQueryable<TEntity, TProperty> queryable)
                : base(queryable) => IncludableInput = queryable;

            internal IIncludableQueryable<TEntity, TProperty> IncludableInput { get; }
        }
    }
}
