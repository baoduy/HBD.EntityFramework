using HBD.EntityFramework.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HBD.EntityFramework.DbContexts.DbRepositories
{
    internal sealed class NotSupportDbDeleting<TEntity> : IDbing<TEntity> where TEntity : IDbEntity
    {
        IDbing<TEntity> IDbing<TEntity>.Include<T>(Func<TEntity, T> selector) => throw new NotSupportedException();
        IDbing<TEntity> IDbing<TEntity>.Includes<T>(Func<TEntity, IEnumerable<T>> selector) => throw new NotImplementedException();

#if NETSTANDARD2_0 || NETSTANDARD1_6
        Task<IDbing<TEntity>> IDbing<TEntity>.IncludeAsync<T>(Func<TEntity, T> selector) => throw new NotImplementedException();
        Task<IDbing<TEntity>> IDbing<TEntity>.IncludesAsync<T>(Func<TEntity, IEnumerable<T>> selector) => throw new NotImplementedException();
#endif
    }

    internal sealed class DbDeleting<TEntity> : IDbing<TEntity> where TEntity : class, IDbEntity
    {
        private readonly IList<IDbEntity> _includedItems;

        public IDbFactory DbFactory { get; }
        public TEntity Entity { get; }

        internal DbDeleting(IDbFactory dbFactory, TEntity entity)
        {
            DbFactory = dbFactory;
            Entity = entity;
            _includedItems = new List<IDbEntity>();
        }

        private void Delete<T>(T entity) where T : class, IDbEntity
        {
            if (_includedItems.Contains(entity)) return;
            _includedItems.Add(entity);

            DbFactory.For<T>().DeleteByKey(entity.GetKeys());
        }

        public IDbing<TEntity> Include<T>(Func<TEntity, T> selector) where T : class, IDbEntity
        {
            var en = selector.Invoke(Entity);
            if (en == null) return this;

            Delete(en);

            return this;
        }

        public IDbing<TEntity> Includes<T>(Func<TEntity, IEnumerable<T>> selector) where T : class, IDbEntity
        {
            var ens = selector.Invoke(Entity);
            if (ens == null) return this;

            foreach (var en in ens)
                Delete(en);

            return this;
        }

#if NETSTANDARD2_0 || NETSTANDARD1_6
        private async Task DeleteAsync<T>(T entity) where T : class, IDbEntity
        {
            if (_includedItems.Contains(entity)) return;
            _includedItems.Add(entity);

            await DbFactory.For<T>().DeleteByKeyAsync(entity.GetKeys());
        }

        public async Task<IDbing<TEntity>> IncludeAsync<T>(Func<TEntity, T> selector) where T : class, IDbEntity
        {
            var en = selector.Invoke(Entity);
            if (en == null) return this;

            await DeleteAsync(en);

            return this;
        }

        public async Task<IDbing<TEntity>> IncludesAsync<T>(Func<TEntity, IEnumerable<T>> selector) where T : class, IDbEntity
        {
            var ens = selector.Invoke(Entity);
            if (ens == null) return this;

            foreach (var en in ens)
                await DeleteAsync(en);

            return this;
        }
#endif

        //public void Dispose() => DbFactory.For<TEntity>().DeleteByKey(Entity.GetKeys());
    }
}
