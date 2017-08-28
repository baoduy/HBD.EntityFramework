#region using

using System;
using System.Threading.Tasks;
using HBD.Framework.Core;
using HBD.EntityFramework.DbContexts.DbEntities;
using System.Linq;
using HBD.Framework.Attributes;
using HBD.EntityFramework.Core;
using System.Reflection;
using HBD.Framework;
using System.Collections.Generic;

#if NETSTANDARD2_0 || NETSTANDARD1_6
using Microsoft.EntityFrameworkCore;
using System.Composition;
#else
using System.Data.Entity;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
#endif

#endregion using

namespace HBD.EntityFramework.DbContexts.DbRepositories
{
    public class DbRepositoryFactory : DbRoRepositoryFactory, IDbRepositoryFactory
    {
        private readonly object _locker = new object();
        private object _byUserNameOrId;

        #region Get-Repositories
        public DbRepositoryFactory(IDbContext dbContext, bool autoDisposeDbContext = false)
            : base(dbContext, autoDisposeDbContext) { }

        public IDbRepository<TEntity> For<TEntity>() where TEntity : class, IDbEntity
            => Cacher.GetOrAdd(typeof(TEntity), k => CreateRepository<TEntity>()) as IDbRepository<TEntity>;

        protected override IDbRoRepository<TEntity> CreateRoRepository<TEntity>()
            => For<TEntity>();

        protected virtual IDbRepository<TEntity> CreateRepository<TEntity>() where TEntity : class, IDbEntity
            => new DbRepository<TEntity>(this, DbContext);
        #endregion

        #region Validation

        /// <summary>
        /// By default DbContext just validate the Primary Key, Foreign Key and Not Null Validation only.
        /// However if you want to apply fully validation for all entities included the custom ValidationAttribute the mark this property is true.
        /// </summary>
        public virtual bool IsApplyFullValidation { get; protected set; } = false;

        /// <summary>
        /// AppyAuditTrail and Validate the Tracking entities.
        /// To customize the validation logic the Validate(object entity) should be overwrite.
        /// To customize the audit trail logic the AppyAuditTrail(object entity) should be overwrite.
        /// </summary>
        private void EntityConsolidation()
        {
            lock (_locker)
            {
                var entities = DbContext.ChangeTracker.Entries()
                    .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted)
                    .Select(e => new EntityStatus(e.Entity, e.State, _byUserNameOrId));

                foreach (var item in entities)
                {
                    OnSaving(item);

                    if (IsApplyFullValidation)
                        Validate(item);
                }
            }
        }

        protected virtual void Validate(EntityStatus entity)
            => Validator.ValidateObject(entity.Entity);

        IList<IDataConventionAdapter> _dataConventionAdapters = new List<IDataConventionAdapter>();
        protected IReadOnlyCollection<IDataConventionAdapter> DataConventionAdapters => _dataConventionAdapters.ToReadOnly();

        protected virtual void OnSaving(EntityStatus entity)
        {
            var CreatedBy = nameof(IDbEntity<dynamic, dynamic>.CreatedBy);
            var CreatedOn = nameof(IDbEntity<dynamic, dynamic>.CreatedOn);

            var UpdatedBy = nameof(IDbEntity<dynamic, dynamic>.UpdatedBy);
            var UpdatedOn = nameof(IDbEntity<dynamic, dynamic>.UpdatedOn);

            //Apply the CreatedOn, CreatedBy and UpdatedOn, UpdatedBy Information.
            if (entity.State == EntityState.Added)
            {
                entity.Entity.SetPropertyValue(CreatedBy, entity.ByUser);
                entity.Entity.SetPropertyValue(CreatedOn, DateTime.Now);
            }
            else
            {
                entity.Entity.SetPropertyValue(UpdatedBy, entity.ByUser);
                entity.Entity.SetPropertyValue(UpdatedOn, DateTime.Now);
            }

            //Calling custom adapter from Mef.
            if (!ServiceLocator.IsServiceLocatorSet) return;
            _dataConventionAdapters.AddRange(ServiceLocator.Current.GetAllInstances<IDataConventionAdapter>());
            if (_dataConventionAdapters.Count <= 0) return;

            //Calling ApplyDataConventionFor Adapters
            var method = typeof(DbRepositoryFactory).GetTypeInfo()
                .GetMethod(nameof(ApplyDataConventionFor),
                BindingFlags.NonPublic | BindingFlags.Instance);

            //Execute the ApplyDataConventionFor method.
            var type = entity.Entity.GetType();
            var md = method.MakeGenericMethod(new[] { type });
            md.Invoke(this, new[] { entity });
        }

        private void ApplyDataConventionFor<TEntity>(EntityStatus entity) where TEntity : class
        {
            var v = _dataConventionAdapters
                .FirstOrDefault(a => a is IDataConventionAdapter<TEntity>) as IDataConventionAdapter<TEntity>;

            if (v == null) return;

            v.ApplyFor(entity.Cast<TEntity>());
        }

        #endregion Validation

        #region Actions
#if NETSTANDARD2_0 || NETSTANDARD1_6
        public void EnsureDbCreated() => DbContext.Database.EnsureCreated();

        public virtual int Save([NotNull]object userName, bool acceptAllChangesOnSuccess = true)
        {
            Guard.ArgumentIsNotNull(userName, nameof(userName));
            _byUserNameOrId = userName;

            EntityConsolidation();
            return DbContext.SaveChanges(acceptAllChangesOnSuccess);
        }

        public virtual Task<int> SaveAsync([NotNull]object userName, bool acceptAllChangesOnSuccess = true)
        {
            Guard.ArgumentIsNotNull(userName, nameof(userName));
            _byUserNameOrId = userName;

            EntityConsolidation();
            return DbContext.SaveChangesAsync(acceptAllChangesOnSuccess);
        }

#else
        public void EnsureDbCreated() => DbContext.Database.CreateIfNotExists();

        public virtual int Save([NotNull]object userName)
        {
            Guard.ArgumentIsNotNull(userName, nameof(userName));
            _byUserNameOrId = userName;

            EntityConsolidation();
            return DbContext.SaveChanges();
        }

        public virtual Task<int> SaveAsync([NotNull]object userName)
        {
            Guard.ArgumentIsNotNull(userName, nameof(userName));
            _byUserNameOrId = userName;

            EntityConsolidation();
            return DbContext.SaveChangesAsync();
        }

        public virtual Task<int> SaveAsync([NotNull]object userName, System.Threading.CancellationToken cancellationToken)
        {
            Guard.ArgumentIsNotNull(userName, nameof(userName));
            _byUserNameOrId = userName;
            EntityConsolidation();
            return DbContext.SaveChangesAsync(cancellationToken);
        }
#endif
        #endregion
    }
}