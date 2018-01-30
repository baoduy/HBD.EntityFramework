using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HBD.EntityFramework.Core;
using HBD.EntityFramework.DbContexts.DbEntities;
using Z.EntityFramework.Plus;
using HBD.Framework.Core;

#if NETSTANDARD2_0 || NETSTANDARD1_6
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
#else
using System.Data.Entity;
using System.Runtime.Caching;
#endif

namespace HBD.EntityFramework.DbContexts.DbRepositories
{
    public class DbRepo<TEntity> : IDbRepo<TEntity> where TEntity : class, IDbEntity
    {
        protected IDbFactory DbFactory { get; }
        protected IDbContext DbContext { get; }

        /// <summary>
        /// Apply default caching policy for 4 hours.
        /// </summary>
        static DbRepo()
        {
#if NETSTANDARD2_0 || NETSTANDARD1_6
            var options = new MemoryCacheEntryOptions() { SlidingExpiration = TimeSpan.FromHours(4) };
            QueryCacheManager.DefaultMemoryCacheEntryOptions = options;
#else
            var options = new CacheItemPolicy() { SlidingExpiration = TimeSpan.FromHours(4) };
            QueryCacheManager.DefaultCacheItemPolicy = options;
#endif
        }

        public DbRepo(IDbFactory dbFactory, IDbContext context)
        {
            Guard.ArgumentIsNotNull(context, nameof(context));
            Guard.ArgumentIsNotNull(dbFactory, nameof(dbFactory));

            DbFactory = dbFactory;
            DbContext = context;
        }

        protected virtual DbSet<TEntity> DbSet => DbContext.Set<TEntity>();

        #region Queries

        public IEnumerable<EntityStatus<TEntity>> GetChangingEntities()
            => this.DbFactory.GetChangingEntities<TEntity>();

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
        public virtual async Task<bool> IsExistedAsync(params object[] keyValues) => (await FindAsync(keyValues)) != null;

        #endregion Getting

        #region Db Actions

        public virtual IDbing<TEntity> Update(TEntity item)
        {
            var entry = DbContext.Entry(item);

            if (entry.State == EntityState.Detached)
            {
                var original = Find(item.GetKeys());
                DbContext.Entry(original).CurrentValues.SetValues(item);
            }

            return new DbUpdating<TEntity>(DbFactory, item);
        }

        public virtual async Task<IDbing<TEntity>> UpdateAsync(TEntity item)
        {
            var entry = DbContext.Entry(item);

            if (entry.State == EntityState.Detached)
            {
                var original = await FindAsync(item.GetKeys());
                DbContext.Entry(original).CurrentValues.SetValues(item);
            }

            return new DbUpdating<TEntity>(DbFactory, item);
        }

        public virtual Task AddAsync(TEntity item)
        {
#if NETSTANDARD2_0 || NETSTANDARD1_6
            return DbSet.AddAsync(item);
#else
            return Task.Run(() => Add(item));
#endif
        }

        public virtual void Add(TEntity item)
        {
            DbSet.Add(item);
        }

        public virtual async Task<IDbing<TEntity>> DeleteAsync(TEntity item)
        {
            var entry = DbContext.Entry(item);

            if (entry.State == EntityState.Detached)
                await DeleteByKeyAsync(item.GetKeys());
            else DbSet.Remove(item);

            return new DbDeleting<TEntity>(DbFactory, item);
        }

        public virtual IDbing<TEntity> Delete(TEntity item)
        {
            //if (this.DbFactory is DbFactory _f)
            //{
            //    var disponsable = new DbDeleting<TEntity>(DbFactory, item);
            //    _f.RegisterDisposableObject(disponsable);
            //    return disponsable;
            //}

            var entry = DbContext.Entry(item);

            if (entry.State == EntityState.Detached)
                DeleteByKey(item.GetKeys());
            else DbSet.Remove(item);

            return new DbDeleting<TEntity>(DbFactory, item);

            return new NotSupportDbDeleting<TEntity>();
        }

        public virtual void DeleteByKey(params object[] keyValues)
            => DbSet.Remove(Find(keyValues));

        public virtual async Task DeleteByKeyAsync(params object[] keyValues)
        {
            var item = await FindAsync(keyValues);
            DbSet.Remove(item);
        }

        /// <summary>
        /// Delete TEntity by condition directly in Database without loading entities. 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual int Delete(Expression<Func<TEntity, bool>> predicate)
            => DbSet.Where(predicate).Delete();

        /// <summary>
        /// Delete TEntity by condition in Database without loading entities. 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
            => DbSet.Where(predicate).DeleteAsync();

        /// <summary>
        /// Delete all in Database without loading entities. 
        /// </summary>
        /// <returns></returns>
        public virtual int DeleteAll() => DbSet.Delete();

        /// <summary>
        /// Delete all in Database without loading entities. 
        /// </summary>
        /// <returns></returns>
        public virtual Task<int> DeleteAllAsync() => DbSet.DeleteAsync();

        #endregion Db Actions
    }
}