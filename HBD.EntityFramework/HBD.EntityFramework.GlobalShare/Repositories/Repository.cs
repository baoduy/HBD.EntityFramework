using HBD.EntityFramework.Core;
using HBD.Framework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

#if NETSTANDARD2_0 || NETSTANDARD1_6
using Microsoft.EntityFrameworkCore;
#else
using System.Data.Entity;
#endif

namespace HBD.EntityFramework.Repositories
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        protected DbContext Context { get; }

        protected Repository(IRepositoryFactory factory, DbContext context)
        {
            Guard.ArgumentIsNotNull(context, nameof(context));
            Guard.ArgumentIsNotNull(factory, nameof(factory));

            RepositoryFactory = factory;
            Context = context;
        }

        protected IRepositoryFactory RepositoryFactory { get; }

        protected virtual DbSet<TEntity> DbSet => Context.Set<TEntity>();

        #region Queries
        public virtual IQueryable<TEntity> AsQueryable(Expression<Func<TEntity, bool>> predicate = null)
        {
            var query = DbSet.AsQueryable();
            if (predicate != null)
                query = query.Where(predicate);
            return query;
        }

        public virtual IOrderedQueryable<TEntity> AsOrderable<TKey>(Expression<Func<TEntity, TKey>> orderBy,
          OrderDirection direction = OrderDirection.Asscending, Expression<Func<TEntity, bool>> predicate = null)
        {
            Guard.ArgumentIsNotNull(orderBy, nameof(orderBy));

            var query = AsQueryable(predicate);
            return direction == OrderDirection.Asscending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);
        }

        public virtual IOrderedQueryable<TEntity> AsOrderable(string orderBy,
            OrderDirection direction = OrderDirection.Asscending, Expression<Func<TEntity, bool>> predicate = null)
        {
            Guard.ArgumentIsNotNull(orderBy, nameof(orderBy));

            var query = AsQueryable(predicate);
            return direction == OrderDirection.Asscending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);
        }
        #endregion

        #region Getting

        public long Count(Expression<Func<TEntity, bool>> predicate = null)
            => AsQueryable(predicate).LongCount();

        public Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate = null)
            => AsQueryable(predicate).LongCountAsync();

        public IEnumerable<TEntity> All(Expression<Func<TEntity, bool>> predicate = null,
            Expression<Func<TEntity, object>> orderBy = null, OrderDirection direction = OrderDirection.Asscending)
        {
            var query = AsQueryable(predicate);
            if (orderBy == null) return query;
            return direction == OrderDirection.Asscending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);
        }

        public Task<List<TEntity>> AllAsync(Expression<Func<TEntity, bool>> predicate = null,
            Expression<Func<TEntity, object>> orderBy = null, OrderDirection direction = OrderDirection.Asscending)
        {
            var query = AsQueryable(predicate);
            if (orderBy == null) return query.ToListAsync();
            return direction == OrderDirection.Asscending
                ? query.OrderBy(orderBy).ToListAsync()
                : query.OrderByDescending(orderBy).ToListAsync();
        }

        public IPagable<TEntity> Page<TKey>(int pageIndex, int pageSize,
            Expression<Func<TEntity, TKey>> orderBy, OrderDirection direction = OrderDirection.Asscending,
            Expression<Func<TEntity, bool>> predicate = null)
        {
            var query = AsOrderable(orderBy, direction, predicate);
            return query.ToPagable(pageIndex, pageSize);
        }

        public Task<IPagable<TEntity>> PageAsync<TKey>(int pageIndex, int pageSize,
            Expression<Func<TEntity, TKey>> orderBy, OrderDirection direction = OrderDirection.Asscending,
            Expression<Func<TEntity, bool>> predicate = null)
        {
            var query = AsOrderable(orderBy, direction, predicate);
            return query.ToPagableAsync(pageIndex, pageSize);
        }

        public virtual TEntity Find(params object[] keyValues) => DbSet.Find(keyValues);

        public virtual Task<TEntity> FindAsync(params object[] keyValues) => DbSet.FindAsync(keyValues);

        #endregion

        #region Db Actions

        public virtual void Update(TEntity item, string updatingUser)
        {
            var entry = Context.Entry(item);

            if (entry.State == EntityState.Detached)
            {
                var original = this.Find(item.GetKeys());
                Context.Entry(original).CurrentValues.SetValues(item);
            }
        }


#if NETSTANDARD2_0 || NETSTANDARD1_6
        public virtual Task AddAsync(TEntity item, string creatingUser)
        {
            item.CreatedBy = creatingUser;
            item.CreatedTime = DateTime.Now;
            return DbSet.AddAsync(item);
        }

        public virtual Task AddRangeAsync(IEnumerable<TEntity> items, string creatingUser)
            => Task.WhenAll(items.Select(i => AddAsync(i, creatingUser)));
#endif

        public virtual void Add(TEntity item, string creatingUser)
        {
            item.CreatedBy = creatingUser;
            item.CreatedTime = DateTime.Now;
            DbSet.Add(item);
        }

        public virtual void AddRange(IEnumerable<TEntity> items, string creatingUser)
        {
            foreach (var item in items)
                Add(item, creatingUser);
        }

        public virtual void UpdateRange(IEnumerable<TEntity> items, string updatingUser)
        {
            foreach (var item in items)
                Update(item, updatingUser);
        }

        public virtual void Delete(TEntity item, string deletingUser) => DbSet.Remove(item);

        public virtual void DeleteRange(IEnumerable<TEntity> items, string deletingUser)
        {
            foreach (var item in items)
                Delete(item, deletingUser);
        }

        public virtual void DeleteByKey(params object[] keyValues) => DbSet.Remove(Find(keyValues));

        public virtual async Task DeleteByKeyAsync(params object[] keyValues)
        {
            var item = await FindAsync(keyValues);
            DbSet.Remove(item);
        }

        public virtual void DeleteWhere(Expression<Func<TEntity, bool>> predicate, string deletingUser)
            => DbSet.RemoveRange(DbSet.Where(predicate));

        public virtual void DeleteAll(string deletingUser) => this.DeleteRange(DbSet, deletingUser);

        #endregion
    }
}
