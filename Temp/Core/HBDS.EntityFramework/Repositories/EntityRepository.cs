#region using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HBDS.EntityFramework.Base;
using HBDS.Framework.Core;
using Microsoft.EntityFrameworkCore;

#endregion

namespace HBDS.EntityFramework.Repositories
{
    public class EntityRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly DbContext _context;

        public EntityRepository(IRepositoryFactory factory, DbContext context)
        {
            Guard.ArgumentIsNotNull(context, nameof(context));
            Guard.ArgumentIsNotNull(factory, nameof(factory));

            RepositoryFactory = factory;
            _context = context;
        }

        protected IRepositoryFactory RepositoryFactory { get; }

        #region Queries

        protected virtual DbSet<TEntity> DbSet => _context.Set<TEntity>();

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

        public virtual void Add(TEntity item, string userName)
        {
            item.CreatedBy = userName;
            item.CreatedTime = DateTime.Now;
            DbSet.Add(item);
        }

        public virtual void AddRange(IEnumerable<TEntity> items, string userName)
        {
            foreach (var item in items)
                Add(item, userName);
        }

        public virtual Task AddAsync(TEntity item, string userName)
        {
            item.CreatedBy = userName;
            item.CreatedTime = DateTime.Now;
            return DbSet.AddAsync(item);
        }

        public virtual Task AddRangeAsync(IEnumerable<TEntity> items, string userName)
            => Task.WhenAll(items.Select(i => AddAsync(i, userName)));

        public virtual void Update(TEntity item, string userName)
        {
            var entry = DbSet.Attach(item);

            entry.Entity.UpdatedBy = userName;
            entry.Entity.UpdatedTime = DateTime.Now;
            //Mark all properties are changes excepts CreatedBy and CreatedTime
            foreach (var property in entry.Properties)
            {
                if (property.Metadata.IsKey()
                    || property.Metadata.Name == nameof(item.CreatedBy)
                    || property.Metadata.Name == nameof(item.CreatedTime))
                {
                    property.IsModified = false;
                    continue;
                }

                property.IsModified = true;
            }
            entry.State = EntityState.Modified;
            DbSet.Update(item);
        }

        public virtual void UpdateRange(IEnumerable<TEntity> items, string userName)
        {
            foreach (var item in items)
                Update(item, userName);
        }

        public virtual void Delete(TEntity item) => DbSet.Remove(item);

        public virtual void DeleteRange(IEnumerable<TEntity> items, string userName)
        {
            foreach (var item in items)
                Delete(item);
        }

        public virtual void DeleteByKey(params object[] keyValues) => DbSet.Remove(Find(keyValues));

        public virtual async Task DeleteByKeyAsync(params object[] keyValues)
        {
            var item = await FindAsync(keyValues);
            DbSet.Remove(item);
        }

        public virtual void DeleteWhere(Expression<Func<TEntity, bool>> predicate)
            => DbSet.RemoveRange(DbSet.Where(predicate));

        #endregion
    }
}