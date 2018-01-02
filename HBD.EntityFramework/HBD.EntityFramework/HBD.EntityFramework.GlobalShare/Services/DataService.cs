using HBD.EntityFramework.Core;
using HBD.EntityFramework.Exceptions;
using HBD.EntityFramework.Services;
using System;
using System.Collections.Generic;

namespace HBD.EntityFramework.Repositories
{
    public abstract class DataService<TEntity, TKey> : IDataService<TEntity, TKey>, IDisposable where TEntity : Aggregate<TKey>
    {
        protected IDbRepositoryFactory DbFactory { get; }

        protected DataService(IDbRepositoryFactory dbFactory)
        {
            DbFactory = dbFactory;
        }

        public abstract int CountAll();
        public abstract TEntity GetById(TKey key);

        /// <summary>
        /// When the entity is locked it is not allow to add/remove any values of the properties.
        /// The InvalidOperationException if this entity is locked.
        /// </summary>
        protected virtual void CheckLocking(TEntity entity)
        {
            if (entity.IsLocking)
                throw new InvalidOperationException($"The {entity.GetType().Name} is locked.", null);
        }

        protected virtual void ValidateKeys(int keyValues)
        {
            if (keyValues <= 0)
                throw new InvaidKeysException(keyValues);
        }

        public TEntity AddOrUpdate(TEntity entity)
        {
            CheckLocking(entity);
            entity.Lock();

            var result = OnAddOrUpdate(entity);
            if (result.EffectedItems > 0)
            {
                DomainEvents.DomainEventsDispatcher.RaiseEvents(entity);
                return result.Entity;
            }

            return null;
        }

        protected abstract ActionResult<TEntity> OnAddOrUpdate(TEntity entity);

        public abstract IReadOnlyCollection<TEntity> All();

        public abstract IPagable<TEntity> GetPage(int pageIndex, int pageSize);

        public abstract bool Delete(TEntity entity);

        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool isDisposing)
        {
            DbFactory.Dispose();
        }
    }
}