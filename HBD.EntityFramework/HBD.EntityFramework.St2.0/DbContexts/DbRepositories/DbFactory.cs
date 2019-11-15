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
using HBD.EntityFramework.Exceptions;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;

#if NETSTANDARD2_0
using Microsoft.EntityFrameworkCore;
using System.Composition;
#else
using System.Data.Entity;

#endif

#endregion using

namespace HBD.EntityFramework.DbContexts.DbRepositories
{
    public class DbFactory : IDbFactory
    {
        private readonly object _locker = new object();
        private object _byUserNameOrId;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContext">The EF DbContext</param>
        /// <param name="autoDisposeDbContext">Once this Factory is being disposed it will dispose the DbContext as well.</param>
        public DbFactory(IDbContext dbContext, bool autoDisposeDbContext = true)
        {
            Guard.ArgumentIsNotNull(dbContext, nameof(dbContext));

            DbContext = dbContext;
            AutoDisposeDbContext = autoDisposeDbContext;
        }

        protected IDbContext DbContext { get; }
        protected bool AutoDisposeDbContext { get; }
        protected ConcurrentDictionary<Type, IDbRepo> Cacher { get; } = new ConcurrentDictionary<Type, IDbRepo>();

        //#region DisposableObjects
        ///// <summary>
        ///// The Disposable Objects that will be disposed before Save.
        ///// </summary>
        //private IList<IDisposable> DisposableObjects = new List<IDisposable>();

        //public void RegisterDisposableObject(IDisposable obj)
        //{
        //    lock (_locker)
        //    {
        //        if (DisposableObjects.Contains(obj))
        //            return;

        //        DisposableObjects.Add(obj);
        //    }
        //}

        //public void RemoveDisposableObject(IDisposable obj)
        //{
        //    lock (_locker)
        //    {
        //        DisposableObjects.Remove(obj);
        //    }
        //}

        //public void ClearDisposableObjects() => DisposableObjects.Clear();

        //private void DisposeObjects()
        //{
        //    lock (_locker)
        //    {
        //        foreach (var item in DisposableObjects)
        //            item.Dispose();

        //        ClearDisposableObjects();
        //    }
        //}
        //#endregion

        public IEnumerable<EntityStatus<TEntity>> GetChangingEntities<TEntity>() where TEntity : class, IDbEntity 
            => DbContext.ChangeTracker.Entries<TEntity>()
            .Select(e => new EntityStatus<TEntity>(e.Entity, e.State, _byUserNameOrId)).ToList();

        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool isDisposing)
        {
            Cacher.Clear();
            //DisposableObjects.Clear();
            if (AutoDisposeDbContext)
                DbContext.Dispose();
        }

        #region Get-Repositories

        public IDbRepo<TEntity> For<TEntity>() where TEntity : class, IDbEntity
            => Cacher.GetOrAdd(typeof(TEntity), k => CreateRepository<TEntity>()) as IDbRepo<TEntity>;

        protected virtual IDbRepo<TEntity> CreateRepository<TEntity>() where TEntity : class, IDbEntity
            => new DbRepo<TEntity>(this, DbContext);
        #endregion

        #region Validation

        /// <summary>
        /// By default DbContext just validate the Primary Key, Foreign Key and Not Null Validation attributes of the Entity only.
        /// However if you want to apply fully validation included the custom ValidationAttribute the mark this property is true.
        /// </summary>
        public virtual bool IsApplyFullValidation { get; protected set; } = false;

        private IList<EntityStatus> _entityStatuses;

        /// <summary>
        /// AppyAuditTrail and Validate the Tracking entities.
        /// To customize the validation logic the Validate(object entity) should be overwrite.
        /// To customize the audit trail logic the AppyAuditTrail(object entity) should be overwrite.
        /// </summary>
        private void RaisePreEvents()
        {
            lock (_locker)
            {
                //Dispose Object before Save
                //this.DisposeObjects();

                try
                {
                    _entityStatuses = DbContext.ChangeTracker.Entries()
                        .Where(e => e.State == EntityState.Added
                            || e.State == EntityState.Modified
                            || e.State == EntityState.Deleted)
                        .Select(e => new EntityStatus(e.Entity, e.State, _byUserNameOrId)).ToList();

                    foreach (var item in _entityStatuses)
                    {
                        OnSaving(item);

                        if (IsApplyFullValidation)
                            Validate(item);
                    }
                }
                catch (ValidationException)
                { throw; }
                catch (Exception ex)
                {
                    _entityStatuses = null;
                    throw new PreSaveException(ex);
                }
            }
        }

        private void RaisePostEvents()
        {
            lock (_locker)
            {
                try
                {
                    foreach (var item in _entityStatuses)
                        OnSaved(item);
                }
                catch (Exception ex)
                {
                    _entityStatuses = null;
                    throw new PostSaveException(ex);
                }
            }
        }

        protected virtual void Validate(EntityStatus entity)
            => Validator.ValidateObject(entity.Entity);

        readonly IList<IPreSaveEventRegister> _preSaveEventRegister = new List<IPreSaveEventRegister>();
        readonly IList<IPostSaveEventRegister> _postSaveEventRegister = new List<IPostSaveEventRegister>();

        protected IReadOnlyCollection<IPreSaveEventRegister> PreSaveEventRegisters => _preSaveEventRegister.ToReadOnly();
        protected IReadOnlyCollection<IPostSaveEventRegister> PostSaveEventRegisters => _postSaveEventRegister.ToReadOnly();

        private bool _eventLoaded = false;
        private void LoadEventRegisters()
        {
            if (_eventLoaded) return;
            if (!ServiceLocator.IsServiceLocatorSet) return;

            _preSaveEventRegister.AddRange(ServiceLocator.Current.GetAllInstances<IPreSaveEventRegister>());
            _postSaveEventRegister.AddRange(ServiceLocator.Current.GetAllInstances<IPostSaveEventRegister>());

            _eventLoaded = true;
        }

        protected virtual void OnSaving(EntityStatus entity)
        {
            const string createdBy = nameof(IDbEntity<dynamic, dynamic>.CreatedBy);
            const string createdOn = nameof(IDbEntity<dynamic, dynamic>.CreatedOn);

            const string updatedBy = nameof(IDbEntity<dynamic, dynamic>.UpdatedBy);
            const string updatedOn = nameof(IDbEntity<dynamic, dynamic>.UpdatedOn);

            //Apply the CreatedOn, CreatedBy and UpdatedOn, UpdatedBy Information.
            if (entity.State == EntityState.Added)
            {
                entity.Entity.SetPropertyValue(createdBy, entity.ByUser);
                entity.Entity.SetPropertyValue(createdOn, DateTime.Now);
            }
            else
            {
                entity.Entity.SetPropertyValue(updatedBy, entity.ByUser);
                entity.Entity.SetPropertyValue(updatedOn, DateTime.Now);
            }

            //Calling custom adapter from Mef.
            this.LoadEventRegisters();
            if (_preSaveEventRegister.Count <= 0) return;

            //Create Method
            var method = typeof(DbFactory).GetTypeInfo()
                .GetMethod(nameof(RaisePreEventsFor),
                BindingFlags.NonPublic | BindingFlags.Instance);

            //Execute the  method.
            var type = entity.Entity.GetType();
            if (method == null) return;

            var md = method.MakeGenericMethod(type);
            md.Invoke(this, new object[] { entity });
        }

        protected virtual void OnSaved(EntityStatus entity)
        {
            this.LoadEventRegisters();
            if (_postSaveEventRegister.Count <= 0) return;

            //Create Method
            var method = typeof(DbFactory).GetTypeInfo()
                .GetMethod(nameof(RaisePostEventsFor),
                    BindingFlags.NonPublic | BindingFlags.Instance);

            //Execute the method.
            var type = entity.Entity.GetType();
            if (method == null) return;

            var md = method.MakeGenericMethod(type);
            md.Invoke(this, new object[] { entity });
        }

        private void RaisePreEventsFor<TEntity>(EntityStatus entity) where TEntity : class
        {
            var v = _preSaveEventRegister
                .FirstOrDefault(a => a is IPreSaveEventRegister<TEntity>) as IPreSaveEventRegister<TEntity>;

            v?.RaiseEvent(this, entity.Cast<TEntity>());
        }

        private void RaisePostEventsFor<TEntity>(EntityStatus entity) where TEntity : class
        {
            var v = _postSaveEventRegister
                .FirstOrDefault(a => a is IPostSaveEventRegister<TEntity>) as IPostSaveEventRegister<TEntity>;

            v?.RaiseEvent(this, entity.Cast<TEntity>());
        }

        #endregion Validation

        #region Actions
#if NETSTANDARD2_0
        public void EnsureDbCreated() => DbContext.Database.EnsureCreated();

        //public virtual int Save([NotNull]object userName, bool acceptAllChangesOnSuccess = true)
        //{
        //    Guard.ArgumentIsNotNull(userName, nameof(userName));
        //    _byUserNameOrId = userName;

        //    RaisePreEvents();
        //    var result = DbContext.SaveChanges(acceptAllChangesOnSuccess);
        //    RaisePostEvents();

        //    return result;
        //}

        //public virtual async Task<int> SaveAsync([NotNull]object userName, bool acceptAllChangesOnSuccess = true)
        //{
        //    Guard.ArgumentIsNotNull(userName, nameof(userName));
        //    _byUserNameOrId = userName;

        //    RaisePreEvents();
        //    var result = await DbContext.SaveChangesAsync(acceptAllChangesOnSuccess);
        //    RaisePostEvents();

        //    return result;
        //}

#else
        public void EnsureDbCreated() => DbContext.Database.CreateIfNotExists();
#endif

        public virtual int Save([NotNull]object byUserNameOrId)
        {
            Guard.ArgumentIsNotNull(byUserNameOrId, nameof(byUserNameOrId));
            _byUserNameOrId = byUserNameOrId;

            RaisePreEvents();
            var result = DbContext.SaveChanges();
            RaisePostEvents();

            return result;
        }

        public virtual async Task<int> SaveAsync([NotNull]object byUserNameOrId)
        {
            Guard.ArgumentIsNotNull(byUserNameOrId, nameof(byUserNameOrId));
            _byUserNameOrId = byUserNameOrId;

            RaisePreEvents();
            var result = await DbContext.SaveChangesAsync();
            RaisePostEvents();

            return result;
        }

        public virtual async Task<int> SaveAsync([NotNull]object byUserNameOrId, System.Threading.CancellationToken cancellationToken)
        {
            Guard.ArgumentIsNotNull(byUserNameOrId, nameof(byUserNameOrId));
            _byUserNameOrId = byUserNameOrId;

            RaisePreEvents();
            var result = await DbContext.SaveChangesAsync(cancellationToken);
            RaisePostEvents();

            return result;
        }

        #endregion
    }
}