using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HBD.EntityFramework.Core;

#if NETSTANDARD2_0 || NETSTANDARD1_6
using Microsoft.EntityFrameworkCore;
#else
using System.Data.Entity;
#endif

namespace HBD.EntityFramework.DbContexts.DbRepositories
{
    public class DbRepository<TEntity> : DbRoRepository<TEntity>, IDbRepository<TEntity> where TEntity : class, IDbEntity
    {
        protected new IDbRepositoryFactory DbFactory => base.DbFactory as IDbRepositoryFactory;

        public DbRepository(IDbRepositoryFactory dbFactory, IDbContext context) : base(dbFactory, context)
        {
        }

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

        public virtual async Task<IDbing<TEntity> > DeleteAsync(TEntity item)
        {
            var entry = DbContext.Entry(item);

            if (entry.State == EntityState.Detached)
                await DeleteByKeyAsync(item.GetKeys());
            else DbSet.Remove(item);

            return new DbDeleting<TEntity>(DbFactory, item);
        }

        public virtual IDbing<TEntity> Delete(TEntity item)
        {
            var entry = DbContext.Entry(item);

            if (entry.State == EntityState.Detached)
                DeleteByKey(item.GetKeys());
            else DbSet.Remove(item);

            return new DbDeleting<TEntity>(DbFactory, item);
        }

        public virtual void DeleteByKey(params object[] keyValues)
            => DbSet.Remove(Find(keyValues));

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