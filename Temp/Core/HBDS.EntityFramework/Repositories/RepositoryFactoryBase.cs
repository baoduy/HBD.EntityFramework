#region using

using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using HBDS.Framework.Core;
using Microsoft.EntityFrameworkCore;

#endregion

namespace HBDS.EntityFramework.Repositories
{
    public abstract class RepositoryFactoryBase : IDisposable
    {
        protected RepositoryFactoryBase(DbContext context, bool autoDisposeDbContext = false)
        {
            Guard.ArgumentIsNotNull(context, nameof(context));
            Context = context;
            AutoDisposeDbContext = autoDisposeDbContext;
        }

        protected DbContext Context { get; }
        protected bool AutoDisposeDbContext { get; }

        protected ConcurrentDictionary<Type, IRepository> Cacher { get; } =
            new ConcurrentDictionary<Type, IRepository>();

        public virtual void Dispose()
        {
            Cacher.Clear();
            if (AutoDisposeDbContext)
                Context.Dispose();
        }

        public virtual int Save(bool acceptAllChangesOnSuccess = true) => Context.SaveChanges(acceptAllChangesOnSuccess);

        public virtual Task<int> SaveAsync(bool acceptAllChangesOnSuccess = true)
            => Context.SaveChangesAsync(acceptAllChangesOnSuccess);
    }
}