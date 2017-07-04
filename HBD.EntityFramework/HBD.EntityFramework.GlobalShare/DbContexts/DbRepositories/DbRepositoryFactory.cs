#region using

using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using HBD.Framework.Core;
using HBD.EntityFramework.DbContexts.Interfaces;
using HBD.EntityFramework.DbContexts.DbEntities;
using System.Linq;

#if NETSTANDARD2_0 || NETSTANDARD1_6

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

#else
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
#endif

#endregion using

namespace HBD.EntityFramework.DbContexts.DbRepositories
{
    public abstract class DbRepositoryFactory : IDbRepositoryFactory
    {
        private readonly object _locker = new object();

        protected DbRepositoryFactory(DbContext context, bool autoDisposeDbContext = false)
        {
            Guard.ArgumentIsNotNull(context, nameof(context));
            DbContext = context;
            AutoDisposeDbContext = autoDisposeDbContext;
        }

        protected DbContext DbContext { get; }
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

        public IDbRepository<TEntity, int, string> For<TEntity>() where TEntity : class, IDbEntity<int, string>
            => Cacher.GetOrAdd(typeof(TEntity), k => CreateRepository<TEntity>()) as IDbRepository<TEntity, int, string>;

        protected abstract IDbRepository<TEntity, int, string> CreateRepository<TEntity>() where TEntity : class, IDbEntity<int, string>;

        #region Validations

        /// <summary>
        /// By default DbContext just validate the Primary Key, Foreign Key and Not Null Validation only.
        /// However if you want to apply fully validation for all entities included the custom ValidationAttribute the mark this property is true.
        /// </summary>
        public virtual bool IsApplyFullValidation { get; protected set; } = false;

        /// <summary>
        /// Validate the Tracking entities.
        /// To customize the validation logic the Validate(object entity) should be overwrite.
        /// </summary>
        protected void Validation()
        {
            if (!IsApplyFullValidation) return;

            lock (_locker)
            {
                var entities = this.DbContext.ChangeTracker.Entries()
                    .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
                    .Select(e => e.Entity);

                foreach (var item in entities)
                    Validate(item);
            }
        }

        protected virtual void Validate(object entity)
        {
            Validator.ValidateObject(entity);
        }

        #endregion Validations

#if NETSTANDARD2_0 || NETSTANDARD1_6

        public void EnsureDbCreated() => DbContext.Database.EnsureCreated();

        public virtual int Save(bool acceptAllChangesOnSuccess = true)
        {
            Validation();
            return DbContext.SaveChanges(acceptAllChangesOnSuccess);
        }

        public virtual Task<int> SaveAsync(bool acceptAllChangesOnSuccess = true)
        {
            Validation();
            return DbContext.SaveChangesAsync(acceptAllChangesOnSuccess);
        }

#else
        public void EnsureDbCreated() => this.DbContext.Database.CreateIfNotExists();

        public virtual int Save()
        {
            Validation();
            return DbContext.SaveChanges();
        }

        public virtual Task<int> SaveAsync()
        {
            Validation();
            return DbContext.SaveChangesAsync();
        }

        public virtual Task<int> SaveAsync(System.Threading.CancellationToken cancellationToken)
        {
            Validation();
            return DbContext.SaveChangesAsync(cancellationToken);
        }
#endif
    }
}