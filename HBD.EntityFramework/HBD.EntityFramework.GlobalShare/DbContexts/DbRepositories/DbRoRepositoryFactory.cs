using HBD.Framework.Core;
using System;
using System.Collections.Concurrent;
using HBD.EntityFramework.Core;

#if NETSTANDARD2_0 || NETSTANDARD1_6
using Microsoft.EntityFrameworkCore;
#else
using System.Data.Entity;
#endif

namespace HBD.EntityFramework.DbContexts.DbRepositories
{

    public class DbRoRepositoryFactory : IDbRoRepositoryFactory
    {
        public DbRoRepositoryFactory(IDbContext dbContext, bool autoDisposeDbContext = false)
        {
            Guard.ArgumentIsNotNull(dbContext, nameof(dbContext));
            DbContext = dbContext;
            AutoDisposeDbContext = autoDisposeDbContext;
        }

        protected IDbContext DbContext { get; }
        protected bool AutoDisposeDbContext { get; }
        protected ConcurrentDictionary<Type, IDbRepository> Cacher { get; } =
           new ConcurrentDictionary<Type, IDbRepository>();

        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool isDisposing)
        {
            Cacher.Clear();
            if (AutoDisposeDbContext)
                DbContext.Dispose();
        }

        public IDbRoRepository<TEntity> ReadOnlyFor<TEntity>() where TEntity : class, IDbEntity
            => Cacher.GetOrAdd(typeof(TEntity), k => CreateRoRepository<TEntity>()) as IDbRoRepository<TEntity>;

        protected virtual IDbRoRepository<TEntity> CreateRoRepository<TEntity>() where TEntity : class, IDbEntity
           => new DbRoRepository<TEntity>(this, DbContext);
    }
}
