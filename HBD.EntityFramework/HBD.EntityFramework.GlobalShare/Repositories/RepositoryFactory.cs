#region using

using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using HBD.Framework.Core;

#if NETSTANDARD2_0 || NETSTANDARD1_6
using Microsoft.EntityFrameworkCore;
#else
using System.Data.Entity;
#endif

#endregion

namespace HBD.EntityFramework.Repositories
{
    public abstract class RepositoryFactory : IDisposable
    {
        protected RepositoryFactory(DbContext context, bool autoDisposeDbContext = false)
        {
            Guard.ArgumentIsNotNull(context, nameof(context));
            Context = context;
            AutoDisposeDbContext = autoDisposeDbContext;
        }

        protected DbContext Context { get; }
        protected bool AutoDisposeDbContext { get; }

        protected ConcurrentDictionary<Type, IRepository> Cacher { get; } =
            new ConcurrentDictionary<Type, IRepository>();

        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool isDisposing)
        {
            Cacher.Clear();
            if (AutoDisposeDbContext)
                Context.Dispose();
        }


#if NETSTANDARD2_0 || NETSTANDARD1_6
        public virtual int Save(bool acceptAllChangesOnSuccess = true) => Context.SaveChanges(acceptAllChangesOnSuccess);

        public virtual Task<int> SaveAsync(bool acceptAllChangesOnSuccess = true)
            => Context.SaveChangesAsync(acceptAllChangesOnSuccess);
#else
        public virtual int Save() => Context.SaveChanges();

        public virtual Task<int> SaveAsync() => Context.SaveChangesAsync();

        public virtual Task<int> SaveAsync(System.Threading.CancellationToken  cancellationToken)
            => Context.SaveChangesAsync(cancellationToken);
#endif
    }
}