using System;
using HBD.Framework.Core;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HBD.EntityFramework.Core;
using Z.EntityFramework.Plus;

#if NETSTANDARD2_0 || NETSTANDARD1_6
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
#else
using System.Data.Entity;
using System.Runtime.Caching;
#endif

namespace HBD.EntityFramework.DbContexts.DbRepositories
{
    public class DbRoRepository<TEntity> : IDbRoRepository<TEntity> where TEntity : class, IDbEntity
    {
        /// <summary>
        /// Apply default caching policy for 4 hours.
        /// </summary>
        static DbRoRepository()
        {
#if NETSTANDARD2_0 || NETSTANDARD1_6
            var options = new MemoryCacheEntryOptions() { SlidingExpiration = TimeSpan.FromHours(4) };
            QueryCacheManager.DefaultMemoryCacheEntryOptions = options;
#else
            var options = new CacheItemPolicy() { SlidingExpiration = TimeSpan.FromHours(4) };
            QueryCacheManager.DefaultCacheItemPolicy = options;
#endif
        }

        protected IDbContext DbContext { get; }

        public DbRoRepository(IDbRoRepositoryFactory dbFactory, IDbContext context)
        {
            Guard.ArgumentIsNotNull(context, nameof(context));
            Guard.ArgumentIsNotNull(dbFactory, nameof(dbFactory));

            DbFactory = dbFactory;
            DbContext = context;
        }

        protected IDbRoRepositoryFactory DbFactory { get; }

        protected virtual DbSet<TEntity> DbSet => DbContext.Set<TEntity>();

        #region Queries

        public virtual IQueryable<TEntity> AsQueryable(Expression<Func<TEntity, bool>> predicate = null)
        {
            var query = DbSet.AsQueryable();
            if (predicate != null)
                query = query.Where(predicate);
            return query;
        }

        public virtual IOrderedQueryable<TEntity> AsOrderable(string orderBy,
            OrderDirection direction = OrderDirection.Asscending, Expression<Func<TEntity, bool>> predicate = null)
        {
            Guard.ArgumentIsNotNull(orderBy, nameof(orderBy));

            var query = AsQueryable(predicate);
            return direction == OrderDirection.Asscending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);
        }

        #endregion Queries

        #region Getting

        private IOrderedQueryable<TEntity> AsOrdered<TKey>(Expression<Func<TEntity, TKey>> orderBy, OrderDirection direction = OrderDirection.Asscending,
            Expression<Func<TEntity, bool>> predicate = null)
        {
            var query = AsQueryable(predicate);
            return direction == OrderDirection.Asscending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);
        }

        public IPagable<TEntity> Page<TKey>(int pageIndex, int pageSize,
            Expression<Func<TEntity, TKey>> orderBy, OrderDirection direction = OrderDirection.Asscending,
            Expression<Func<TEntity, bool>> predicate = null)
        {
            var query = AsOrdered(orderBy, direction, predicate);
            return query.ToPagable(pageIndex, pageSize);
        }

        public Task<IPagable<TEntity>> PageAsync<TKey>(int pageIndex, int pageSize,
            Expression<Func<TEntity, TKey>> orderBy, OrderDirection direction = OrderDirection.Asscending,
            Expression<Func<TEntity, bool>> predicate = null)
        {
            var query = AsOrdered(orderBy, direction, predicate);
            return query.ToPagableAsync(pageIndex, pageSize);
        }

        public virtual TEntity Find(params object[] keyValues) => DbSet.Find(keyValues);

        public virtual Task<TEntity> FindAsync(params object[] keyValues) => DbSet.FindAsync(keyValues);

        public virtual bool IsExisted(params object[] keyValues) => Find(keyValues) != null;

        #endregion Getting
    }
}
