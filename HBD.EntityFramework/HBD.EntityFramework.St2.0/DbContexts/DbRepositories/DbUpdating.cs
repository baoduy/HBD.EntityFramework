using HBD.EntityFramework.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HBD.EntityFramework.DbContexts.DbRepositories
{
    internal sealed class NotSupportDbUpdating<TEntity> : IDbing<TEntity> where TEntity : IDbEntity
    {
        IDbing<TEntity> IDbing<TEntity>.Include<T>(Func<TEntity, T> selector) => throw new NotSupportedException();
        IDbing<TEntity> IDbing<TEntity>.Includes<T>(Func<TEntity, IEnumerable<T>> selector) => throw new NotSupportedException();

#if NETSTANDARD2_0
        Task<IDbing<TEntity>> IDbing<TEntity>.IncludeAsync<T>(Func<TEntity, T> selector) => throw new NotSupportedException();
        Task<IDbing<TEntity>> IDbing<TEntity>.IncludesAsync<T>(Func<TEntity, IEnumerable<T>> selector) => throw new NotSupportedException();
#endif
    }

    internal sealed class DbUpdating<TEntity> : IDbing<TEntity> where TEntity : IDbEntity
    {
        private readonly IList<IDbEntity> _includedItems;

        public IDbFactory DbFactory { get; }
        public TEntity Entity { get; }

        internal DbUpdating(IDbFactory dbFactory, TEntity entity)
        {
            DbFactory = dbFactory;
            Entity = entity;
            _includedItems = new List<IDbEntity>();
        }

        private void AddOrUpdate<T>(T entity) where T : class, IDbEntity
        {
            if (_includedItems.Contains(entity)) return;
            _includedItems.Add(entity);

            var repo = DbFactory.For<T>();

            if (repo.IsExisted(entity.GetKeys()))
                repo.Update(entity);
            else
                repo.Add(entity);
        }

        public IDbing<TEntity> Include<T>(Func<TEntity, T> selector) where T : class, IDbEntity
        {
            var en = selector.Invoke(Entity);
            if (en == null) return this;

            AddOrUpdate(en);

            return this;
        }

        public IDbing<TEntity> Includes<T>(Func<TEntity, IEnumerable<T>> selector) where T : class, IDbEntity
        {
            var ens = selector.Invoke(Entity);
            if (ens == null) return this;

            foreach (var en in ens)
                AddOrUpdate(en);

            return this;
        }

#if NETSTANDARD2_0
        private async Task AddOrUpdateAsync<T>(T entity) where T : class, IDbEntity
        {
            if (_includedItems.Contains(entity)) return;
            _includedItems.Add(entity);

            var repo = DbFactory.For<T>();

            if (await repo.IsExistedAsync(entity.GetKeys()))
                await repo.UpdateAsync(entity);
            else
                await repo.AddAsync(entity);
        }

        public async Task<IDbing<TEntity>> IncludeAsync<T>(Func<TEntity, T> selector) where T : class, IDbEntity
        {
            var en = selector.Invoke(Entity);
            if (en == null) return this;

            await AddOrUpdateAsync(en);

            return this;
        }

        public async Task<IDbing<TEntity>> IncludesAsync<T>(Func<TEntity, IEnumerable<T>> selector) where T : class, IDbEntity
        {
            var ens = selector.Invoke(Entity);
            if (ens == null) return this;

            foreach (var en in ens)
                await AddOrUpdateAsync(en);

            return this;
        }
#endif
    }
}
