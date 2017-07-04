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

#else
using System.Data.Entity;
#endif

#endregion using

namespace HBD.EntityFramework.DbContexts.BasicRepositories
{
    public abstract class DbBasicRepositoryFactory : IDbBasicRepositoryFactory
    {
        private readonly object _locker = new object();

        protected DbBasicRepositoryFactory(DbContext context, bool autoDisposeDbContext = false)
        {
            Guard.ArgumentIsNotNull(context, nameof(context));
            Context = context;
            AutoDisposeDbContext = autoDisposeDbContext;
        }

        protected DbContext Context { get; }
        protected bool AutoDisposeDbContext { get; }

        protected ConcurrentDictionary<Type, IDbRepository> Cacher { get; } =
            new ConcurrentDictionary<Type, IDbRepository>();

        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool isDisposing)
        {
            Cacher.Clear();
            if (AutoDisposeDbContext)
                Context.Dispose();
        }

        public IDbBasicRepository<TEntity> For<TEntity>() where TEntity : class, IDbEntity
            => Cacher.GetOrAdd(typeof(TEntity), k => CreateRepository<TEntity>()) as IDbBasicRepository<TEntity>;

        protected abstract IDbBasicRepository<TEntity> CreateRepository<TEntity>() where TEntity : class, IDbEntity;

        #region Validation

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
                var entities = this.Context.ChangeTracker.Entries()
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

        #endregion Validation

#if NETSTANDARD2_0 || NETSTANDARD1_6

        public void EnsureDbCreated() => Context.Database.EnsureCreated();

        public virtual int Save(bool acceptAllChangesOnSuccess = true)
        {
            Validation();
            return Context.SaveChanges(acceptAllChangesOnSuccess);
        }

        public virtual Task<int> SaveAsync(bool acceptAllChangesOnSuccess = true)
        {
            Validation();
            return Context.SaveChangesAsync(acceptAllChangesOnSuccess);
        }

#else
        public void EnsureDbCreated() => Context.Database.CreateIfNotExists();

        public virtual int Save()
        {
            Validation();
            return Context.SaveChanges();
        }

        public virtual Task<int> SaveAsync()
        {
            Validation();
            return Context.SaveChangesAsync();
        }

        public virtual Task<int> SaveAsync(System.Threading.CancellationToken cancellationToken)
        {
            Validation();
            return Context.SaveChangesAsync(cancellationToken);
        }
#endif
    }
}