using HBD.Framework.Core;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HBD.Framework.Attributes;
using HBD.EntityFramework.DbContexts.DbEntities;
using HBD.EntityFramework.DbContexts.Interfaces;

#if NETSTANDARD2_0 || NETSTANDARD1_6

using Microsoft.EntityFrameworkCore;

#else
using System.Data.Entity;
#endif

namespace HBD.EntityFramework.DbContexts.BasicRepositories
{
    public abstract class DbBasicRepository<TEntity> : IDbBasicRepository<TEntity> where TEntity : class, IDbEntity
    {
        protected DbContext DbContext { get; }

        protected DbBasicRepository(IDbBasicRepositoryFactory dbFactory, DbContext context)
        {
            Guard.ArgumentIsNotNull(context, nameof(context));
            Guard.ArgumentIsNotNull(dbFactory, nameof(dbFactory));

            DbFactory = dbFactory;
            DbContext = context;
        }

        protected IDbBasicRepositoryFactory DbFactory { get; }

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

        public bool IsExisted([NotNull]TEntity item)
        {
            Guard.ArgumentIsNotNull(item, nameof(item));
            return IsExisted(item.GetKeys());
        }

        public virtual bool IsExisted(params object[] keyValues) => this.Find(keyValues) != null;

        #endregion Getting

        #region Db Actions

        public virtual void Update(TEntity item)
        {
            var entry = DbContext.Entry(item);

            if (entry.State == EntityState.Detached)
            {
                var original = this.Find(item.GetKeys());
                DbContext.Entry(original).CurrentValues.SetValues(item);
            }
        }

        public virtual async Task UpdateAsync(TEntity item)
        {
            var entry = DbContext.Entry(item);

            if (entry.State == EntityState.Detached)
            {
                var original = await this.FindAsync(item.GetKeys());
                DbContext.Entry(original).CurrentValues.SetValues(item);
            }
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

        public virtual void Delete(TEntity item) => DbSet.Remove(item);

        public virtual void DeleteByKey(params object[] keyValues) => DbSet.Remove(Find(keyValues));

        public virtual async Task DeleteByKeyAsync(params object[] keyValues)
        {
            var item = await FindAsync(keyValues);
            DbSet.Remove(item);
        }

        public virtual void DeleteWhere(Expression<Func<TEntity, bool>> predicate)
            => DbSet.RemoveRange(DbSet.Where(predicate));

        public virtual void DeleteAll() => DbSet.RemoveRange(DbSet);

        #endregion Db Actions
    }
}