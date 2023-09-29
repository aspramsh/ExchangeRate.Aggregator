using System.Linq.Expressions;

namespace ExchangeRate.Aggregator.Shared.Infrastructure.Repositories
{
    public interface IEntityFrameworkGenericRepository<TEntity>
        where TEntity : class
    {
        void DetachLocal(Func<TEntity, bool> predicate);

        IQueryable<TEntity> Query();

        Task<bool> AnyAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default);

        Task<TEntity> GetOneAsync(
            Expression<Func<TEntity, bool>> predicate,
            Func<IIncludable<TEntity>, IIncludable> includes = default,
            bool isTracked = false,
            CancellationToken cancellationToken = default);

        IQueryable<TEntity> Get(
            Expression<Func<TEntity, bool>> predicate,
            bool isTracked = false,
            params string[] includes);

        Task<IEnumerable<TEntity>> FindByAsync(
            Expression<Func<TEntity, bool>> predicate,
            Func<IIncludable<TEntity>, IIncludable> includes = default,
            string orderBy = default,
            CancellationToken cancellationToken = default);

        Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task InsertRangeAsync(List<TEntity> entities, CancellationToken cancellationToken = default);

        void Update(TEntity entity);

        void UpdatePartially(TEntity entity, IEnumerable<string> updatedProperties, bool isAttached = false);

        void UpdateAttached(TEntity entity);

        void UpdateRange(List<TEntity> entities);

        void Delete(TEntity entity);

        void DeleteByRange(IEnumerable<TEntity> entities);

        Task UpsertAsync(
            TEntity entity,
            Expression<Func<TEntity, object>> match,
            CancellationToken cancellationToken = default);

        Task UpsertRangeAsync(
            IEnumerable<TEntity> entities,
            Expression<Func<TEntity, object>> match,
            CancellationToken cancellationToken = default);

        Task<PagedResultEntity<TEntity>> GetPagedByAsync(
            int pageNumber,
            int pageSize,
            Expression<Func<TEntity, bool>> predicate = default,
            Func<IIncludable<TEntity>, IIncludable> includes = default,
            string orderBy = default,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<TEntity>> FromSqlInterpolatedAsync(
            FormattableString query,
            CancellationToken cancellationToken);

        Task<IEnumerable<TEntity>> FromSqlRawAsync(
            string query,
            CancellationToken cancellationToken);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
