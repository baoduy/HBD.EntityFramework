using HBD.Framework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HBD.Framework.Attributes;
using HBD.EntityFramework.DbContexts.Interfaces;
using HBD.EntityFramework.DbContexts.DbEntities;
using HBD.EntityFramework.Core;
using HBD.EntityFramework.Exceptions;

#if NETSTANDARD2_0 || NETSTANDARD1_6

using Microsoft.EntityFrameworkCore;

#else
using System.Data.Entity;
#endif

namespace HBD.EntityFramework.DbContexts.DbRepositories
{
    public abstract class DbRepository<TEntity> : IDbRepository<TEntity, int, string> where TEntity : class, IDbEntity<int, string>
    {
        protected DbContext DbContext { get; }

        protected DbRepository(DbRepositoryFactory dbFactory, DbContext context)
        {
            Guard.ArgumentIsNotNull(context, nameof(context));
            Guard.ArgumentIsNotNull(dbFactory, nameof(dbFactory));

            DbFactory = dbFactory;
            DbContext = context;
        }

        protected DbRepositoryFactory DbFactory { get; }

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

        private IOrderedQueryable<TEntity> AsOrdered(Expression<Func<TEntity, int>> orderBy, OrderDirection direction = OrderDirection.Asscending,
            Expression<Func<TEntity, bool>> predicate = null)
        {
            var query = AsQueryable(predicate);
            return direction == OrderDirection.Asscending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);
        }

        public IPagable<TEntity> Page(int pageIndex, int pageSize,
            Expression<Func<TEntity, int>> orderBy, OrderDirection direction = OrderDirection.Asscending,
            Expression<Func<TEntity, bool>> predicate = null)
        {
            var query = AsOrdered(orderBy, direction, predicate);
            return query.ToPagable(pageIndex, pageSize);
        }

        public Task<IPagable<TEntity>> PageAsync(int pageIndex, int pageSize,
            Expression<Func<TEntity, int>> orderBy, OrderDirection direction = OrderDirection.Asscending,
            Expression<Func<TEntity, bool>> predicate = null)
        {
            var query = AsOrdered(orderBy, direction, predicate);
            return query.ToPagableAsync(pageIndex, pageSize);
        }

        public virtual TEntity Find(int keyValues) => DbSet.Find(keyValues);

        public virtual Task<TEntity> FindAsync(int keyValues) => DbSet.FindAsync(keyValues);

        public bool IsExisted([NotNull]TEntity item)
        {
            Guard.ArgumentIsNotNull(item, nameof(item));
            return IsExisted(item.Id);
        }

        public virtual bool IsExisted(int keyValues) => this.Find(keyValues) != null;

        #endregion Getting

        #region Db Actions

        #region Update
        public virtual IDbUpdating<TEntity, int, string> Update(TEntity item, string updatingUser)
        {
            item.UpdatedTime = DateTime.Now;
            item.UpdatedBy = updatingUser;

            var entry = DbContext.Entry(item);

            if (entry.State == EntityState.Detached)
            {
                var original = this.Find(item.Id);
                DbContext.Entry(original).CurrentValues.SetValues(item);
            }

            return new DbEntityUpdating<TEntity>(this.DbFactory, item, updatingUser);
        }

        public virtual async Task<IDbUpdating<TEntity, int, string>> UpdateAsync(TEntity item, string updatingUser)
        {
            item.UpdatedTime = DateTime.Now;
            item.UpdatedBy = updatingUser;

            var entry = DbContext.Entry(item);

            if (entry.State == EntityState.Detached)
            {
                var original = await this.FindAsync(item.Id);
                DbContext.Entry(original).CurrentValues.SetValues(item);
            }

            return new DbEntityUpdating<TEntity>(this.DbFactory, item, updatingUser);
        }
        #endregion

        #region Add
        public virtual Task AddAsync(TEntity item, string creatingUser)
        {
#if NETSTANDARD2_0 || NETSTANDARD1_6
            item.CreatedBy = creatingUser;
            item.CreatedTime = DateTime.Now;
            return DbSet.AddAsync(item);
#else
            return Task.Run(() => Add(item, creatingUser));
#endif
        }

        public virtual void Add(TEntity item, string creatingUser)
        {
            item.CreatedBy = creatingUser;
            item.CreatedTime = DateTime.Now;
            DbSet.Add(item);
        }
        #endregion

        #region Delete
        public virtual void Delete(TEntity item, string deletingUser) => DbSet.Remove(item);

        public virtual void DeleteRange(IEnumerable<TEntity> items, string deletingUser)
        {
            foreach (var item in items)
                Delete(item, deletingUser);
        }

        protected virtual void ValidateKeys(int keyValues)
        {
            if (keyValues <= 0)
                throw new InvaidKeysException(keyValues);
        }

        public virtual void DeleteByKey(int keyValues)
        {
            this.ValidateKeys(keyValues);
            var entity = Find(keyValues);
            if (entity == null)
                throw new EntityNotFoundException(keyValues);
            DbSet.Remove(entity);
        }

        public virtual async Task DeleteByKeyAsync(int keyValues)
        {
            this.ValidateKeys(keyValues);
            var item = await FindAsync(keyValues);
            DbSet.Remove(item);
        }

        public virtual void DeleteWhere(Expression<Func<TEntity, bool>> predicate, string deletingUser)
            => DbSet.RemoveRange(DbSet.Where(predicate));

        public virtual void DeleteAll(string deletingUser) => this.DeleteRange(DbSet, deletingUser);
        #endregion

        #endregion Db Actions
    }
}