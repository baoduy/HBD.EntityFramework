using System;
using System.Collections.Generic;
using HBD.Framework.Attributes;
using System.Threading.Tasks;
using System.Threading;

#if NETSTANDARD2_0
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
#else
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
#endif

namespace HBD.EntityFramework.DbContexts
{
    public interface IDbContext : IDisposable
    {
#if NETSTANDARD2_0
        DatabaseFacade Database { get;}
        ChangeTracker ChangeTracker { get;}
        EntityEntry<TEntity> Entry<TEntity>([NotNull] TEntity entity) where TEntity : class;
        EntityEntry Entry([NotNull] object entity);

        EntityEntry<TEntity> Update<TEntity>([NotNull] TEntity entity) where TEntity : class;
        EntityEntry Update([NotNull] object entity);
        void UpdateRange([NotNull] params object[] entities);
        void UpdateRange([NotNull] IEnumerable<object> entities);

        //int SaveChanges(bool acceptAllChangesOnSuccess);
        //Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess);
        //Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken);
#else
        Database Database { get;}
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        DbEntityEntry Entry([NotNull] object entity);
        DbSet Set(Type entityType);
        DbChangeTracker ChangeTracker { get; }
#endif

        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        int SaveChanges();
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
