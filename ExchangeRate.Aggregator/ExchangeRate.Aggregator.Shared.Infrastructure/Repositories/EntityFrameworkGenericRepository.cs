using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ExchangeRate.Aggregator.Shared.Infrastructure.Repositories
{
    public abstract class EntityFrameworkGenericRepository<TEntity> : IEntityFrameworkGenericRepository<TEntity>
        where TEntity : class
    {
        protected readonly DbContext DbContext;
        
        protected readonly ILogger Logger;

        protected EntityFrameworkGenericRepository(
            ILoggerFactory loggerFactory,
            DbContext dbContext)
        {
            Logger = loggerFactory.CreateLogger<EntityFrameworkGenericRepository<TEntity>>();
            DbContext = dbContext;
        }

        public IQueryable<TEntity> Query()
        {
            return DbContext.Set<TEntity>().AsQueryable();
        }

        public virtual async Task<bool> AnyAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            return await Query().AnyAsync(predicate, cancellationToken);
        }

        public virtual async Task<TEntity> GetOneAsync(
            Expression<Func<TEntity, bool>> predicate,
            Func<IIncludable<TEntity>, IIncludable> includes = default,
            bool isTracked = false,
            CancellationToken cancellationToken = default)
        {
            var query = Query();

            query = isTracked ? query.AsTracking() : query.AsNoTracking();

            query = query.IncludeMultiple(includes);

            return await query.FirstOrDefaultAsync(predicate, cancellationToken: cancellationToken);
        }

        public IQueryable<TEntity> Get(
            Expression<Func<TEntity, bool>> predicate,
            bool isTracked = false,
            params string[] includes)
        {
            IQueryable<TEntity> set = DbContext.Set<TEntity>();

            set = isTracked ? set.AsTracking() : set.AsNoTracking();

            for (int i = 0; i < includes.Length; i++)
            {
                set = set.Include(includes[i]);
            }

            if (predicate == null)
            {
                return set;
            }

            return set.Where(predicate);
        }

        public virtual async Task<IEnumerable<TEntity>> FindByAsync(
            Expression<Func<TEntity, bool>> predicate,
            Func<IIncludable<TEntity>, IIncludable> includes = default,
            string orderBy = default,
            CancellationToken cancellationToken = default)
        {
            var query = DbContext.Set<TEntity>().AsNoTracking();

            if (predicate != default)
            {
                query = query.Where(predicate);
            }

            if (includes != default)
            {
                query = query.IncludeMultiple(includes);
            }

            if (orderBy != default)
            {
                query = query.OrderBy(orderBy);
            }

            return await query.ToListAsync(cancellationToken);
        }

        public virtual async Task<PagedResultEntity<TEntity>> GetPagedByAsync(
            int pageNumber,
            int pageSize,
            Expression<Func<TEntity, bool>> predicate = default,
            Func<IIncludable<TEntity>, IIncludable> includes = default,
            string orderBy = default,
            CancellationToken cancellationToken = default)
        {
            predicate ??= PredicateBuilder.New<TEntity>(true);

            var pagedResult = new PagedResultEntity<TEntity>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = await DbContext.Set<TEntity>().CountAsync(predicate, cancellationToken),
            };

            var pageCount = (double)pagedResult.TotalRecords / pageSize;

            if (pagedResult.TotalRecords == 0)
            {
                return pagedResult;
            }

            if (pageCount > 0 && Math.Ceiling(pageCount - pageNumber) < 0)
            {
                pagedResult.TotalRecords = default;
                return pagedResult;
            }

            pagedResult.TotalPages = (int)Math.Ceiling(pageCount);

            var skip = (pageNumber - 1) * pageSize;

            var query = DbContext.Set<TEntity>().AsNoTracking().Where(predicate);

            if (includes != default)
            {
                query = query.IncludeMultiple(includes);
            }

            if (orderBy != default)
            {
                query = query.OrderBy(orderBy);
            }

            query = query
                .Skip(skip)
                .Take(pageSize);

            pagedResult.Results = await query.ToListAsync(cancellationToken);

            return pagedResult;
        }

        #region Insert

        public virtual async Task<TEntity> InsertAsync(
            TEntity entity,
            CancellationToken cancellationToken = default)
        {
            var data = await DbContext.Set<TEntity>().AddAsync(entity, cancellationToken);
            return data.Entity;
        }

        /// <inheritdoc />
        /// <summary>
        /// Insert by range.
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task InsertRangeAsync(
            List<TEntity> entities,
            CancellationToken cancellationToken = default)
        {
            await DbContext.Set<TEntity>().AddRangeAsync(entities, cancellationToken);
        }

        #endregion Insert

        #region Update

        public virtual void Update(TEntity entity)
        {
            DbContext.Entry(entity).State = EntityState.Modified;
            DbContext.Set<TEntity>().Update(entity);
        }

        public virtual void UpdatePartially(
            TEntity entity,
            IEnumerable<string> updatedProperties,
            bool isAttachable = false)
        {
            if (isAttachable)
            {
                DbContext.Attach(entity);
            }

            var keyNames = DbContext.Model
                .FindEntityType(typeof(TEntity))
                .FindPrimaryKey().Properties
                .Select(x => x.Name)
                .ToArray();

            foreach (var item in updatedProperties)
            {
                if (!keyNames.Contains(item))
                {
                    DbContext.Entry(entity).Property(item).IsModified = true;
                }
            }
        }

        public virtual void UpdateAttached(TEntity entity)
        {
            DbContext.Attach(entity);
            DbContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void UpdateRange(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                DbContext.Entry(entity).State = EntityState.Modified;
            }

            DbContext.Set<TEntity>().UpdateRange(entities);
        }

        public virtual async Task<IEnumerable<TEntity>> FromSqlInterpolatedAsync(
            FormattableString query,
            CancellationToken cancellationToken)
        {
            var entities = await DbContext
                .Set<TEntity>()
                .FromSqlInterpolated(query)
                .ToListAsync(cancellationToken);

            return entities;
        }

        public virtual async Task<IEnumerable<TEntity>> FromSqlRawAsync(
            string query,
            CancellationToken cancellationToken)
        {
            var entities = await DbContext
                .Set<TEntity>()
                .FromSqlRaw(query)
                .ToListAsync(cancellationToken);

            return entities;
        }

        #endregion Update

        #region Delete

        public virtual void Delete(TEntity entity)
        {
            DbContext.Entry(entity).State = EntityState.Deleted;
            DbContext.Set<TEntity>().Remove(entity);
        }

        public virtual void DeleteByRange(IEnumerable<TEntity> entities)
        {
            var removeRange = entities as TEntity[] ?? entities.ToArray();

            foreach (var entity in removeRange)
            {
                DbContext.Entry(entity).State = EntityState.Deleted;
            }

            DbContext.Set<TEntity>().RemoveRange(removeRange);
        }

        #endregion Delete

        #region Upsert

        public virtual async Task UpsertAsync(
            TEntity entity,
            Expression<Func<TEntity, object>> match,
            CancellationToken cancellationToken = default)
        {
            await DbContext.Set<TEntity>().Upsert(entity)
                .On(match)
                .RunAsync(cancellationToken);
        }

        public virtual async Task UpsertRangeAsync(
            IEnumerable<TEntity> entities,
            Expression<Func<TEntity, object>> match,
            CancellationToken cancellationToken = default)
        {
            await DbContext.Set<TEntity>().UpsertRange(entities)
                .On(match)
                .RunAsync(cancellationToken);
        }

        #endregion Upsert

        public void DetachLocal(Func<TEntity, bool> predicate)
        {
            var local = DbContext.Set<TEntity>()
                .Local
                .FirstOrDefault(predicate);

            if (local != default)
            {
                DbContext.Entry(local).State = EntityState.Detached;
            }
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await DbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
