using HBD.EntityFramework.DbContexts.DbEntities;
using HBD.EntityFramework.DbContexts.Interfaces;
using HBD.Framework.Core;
using System;
using System.Collections.Concurrent;

#if NETSTANDARD2_0 || NETSTANDARD1_6
using Microsoft.EntityFrameworkCore;
#else
using System.Data.Entity;
#endif

namespace HBD.EntityFramework.DbContexts.DbRepositories
{

    public class DbReadOnlyRepoFactory : IDbReadOnlyRepoFactory
    {
        public DbReadOnlyRepoFactory(DbContext dbContext, bool autoDisposeDbContext = false)
        {
            Guard.ArgumentIsNotNull(dbContext, nameof(dbContext));
            DbContext = dbContext;
            AutoDisposeDbContext = autoDisposeDbContext;
        }

        protected DbContext DbContext { get; }
        protected bool AutoDisposeDbContext { get; }
        protected ConcurrentDictionary<Type, IDbRepo> Cacher { get; } =
           new ConcurrentDictionary<Type, IDbRepo>();

        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool isDisposing)
        {
            Cacher.Clear();
            if (AutoDisposeDbContext)
                DbContext.Dispose();
        }

        public IDbReadOnlyRepo<TEntity> ReadOnlyFor<TEntity>() where TEntity : class, IDbEntity
            => Cacher.GetOrAdd(typeof(TEntity), k => CreateReadOnlyRepo<TEntity>()) as IDbReadOnlyRepo<TEntity>;

        protected virtual IDbReadOnlyRepo<TEntity> CreateReadOnlyRepo<TEntity>() where TEntity : class, IDbEntity
           => new DbReadOnlyRepo<TEntity>(this, this.DbContext);
    }
}
