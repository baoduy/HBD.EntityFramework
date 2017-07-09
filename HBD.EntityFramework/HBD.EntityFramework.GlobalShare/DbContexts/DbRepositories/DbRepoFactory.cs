#region using

using System;
using System.Threading.Tasks;
using HBD.Framework.Core;
using HBD.EntityFramework.DbContexts.Interfaces;
using HBD.EntityFramework.DbContexts.DbEntities;
using System.Linq;
using HBD.Framework.Attributes;

#if NETSTANDARD2_0 || NETSTANDARD1_6

using Microsoft.EntityFrameworkCore;

#else
using System.Data.Entity;
#endif

#endregion using

namespace HBD.EntityFramework.DbContexts.DbRepositories
{
    public class DbRepoFactory : DbReadOnlyRepoFactory, IDbRepoFactory
    {
        private readonly object _locker = new object();
        private string _userActionName;

        public DbRepoFactory(DbContext dbContext, bool autoDisposeDbContext = false)
            : base(dbContext, autoDisposeDbContext) { }

        public IDbRepo<TEntity> For<TEntity>() where TEntity : class, IDbEntity
            => Cacher.GetOrAdd(typeof(TEntity), k => CreateRepo<TEntity>()) as IDbRepo<TEntity>;

        protected override IDbReadOnlyRepo<TEntity> CreateReadOnlyRepo<TEntity>()
            => For<TEntity>();

        protected virtual IDbRepo<TEntity> CreateRepo<TEntity>() where TEntity : class, IDbEntity
            => new DbRepo<TEntity>(this, this.DbContext);

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
                var entities = this.DbContext.ChangeTracker.Entries()
                    .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted)
                    .Select(e => new EntityStatus(e.Entity, e.State));

                foreach (var item in entities)
                {
                    AppyAuditTrail(item, _userActionName);

                    if (IsApplyFullValidation)
                        Validate(item);
                }
            }
        }

        protected virtual void Validate(EntityStatus entity)
        {
            Validator.ValidateObject(entity.Entity);
        }

        protected virtual void AppyAuditTrail(EntityStatus entity, string user)
        {
            switch (entity.Entity)
            {
                case DbEntity<int> e:
                    {
                        if (entity.State == EntityState.Added)
                        {
                            e.CreatedBy = user;
                            e.CreatedTime = DateTime.Now;
                        }
                        else
                        {
                            e.UpdatedBy = user;
                            e.UpdatedTime = DateTime.Now;
                        }
                    }
                    break;
                case DbEntity<string> e:
                    {
                        if (entity.State == EntityState.Added)
                        {
                            e.CreatedBy = user;
                            e.CreatedTime = DateTime.Now;
                        }
                        else
                        {
                            e.UpdatedBy = user;
                            e.UpdatedTime = DateTime.Now;
                        }
                    }
                    break;
            }
        }

        #endregion Validation

#if NETSTANDARD2_0 || NETSTANDARD1_6

        public void EnsureDbCreated() => DbContext.Database.EnsureCreated();

        public virtual int Save([NotNull]string userName, bool acceptAllChangesOnSuccess = true)
        {
            Guard.ArgumentIsNotNull(userName, nameof(userName));
            _userActionName = userName;

            EntityConsolidation();
            return DbContext.SaveChanges(acceptAllChangesOnSuccess);
        }

        public virtual Task<int> SaveAsync([NotNull]string userName, bool acceptAllChangesOnSuccess = true)
        {
            Guard.ArgumentIsNotNull(userName, nameof(userName));
            _userActionName = userName;

            EntityConsolidation();
            return DbContext.SaveChangesAsync(acceptAllChangesOnSuccess);
        }

#else
        public void EnsureDbCreated() => DbContext.Database.CreateIfNotExists();

        public virtual int Save([NotNull]string userName)
        {
            Guard.ArgumentIsNotNull(userName, nameof(userName));
            _userActionName = userName;

            EntityConsolidation();
            return DbContext.SaveChanges();
        }

        public virtual Task<int> SaveAsync([NotNull]string userName)
        {
            Guard.ArgumentIsNotNull(userName, nameof(userName));
            _userActionName = userName;

            EntityConsolidation();
            return DbContext.SaveChangesAsync();
        }

        public virtual Task<int> SaveAsync([NotNull]string userName, System.Threading.CancellationToken cancellationToken)
        {
            Guard.ArgumentIsNotNull(userName, nameof(userName));
            _userActionName = userName;
            EntityConsolidation();
            return DbContext.SaveChangesAsync(cancellationToken);
        }
#endif
    }
}