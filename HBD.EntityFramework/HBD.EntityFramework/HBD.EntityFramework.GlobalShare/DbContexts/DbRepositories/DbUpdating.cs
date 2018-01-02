using HBD.EntityFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HBD.EntityFramework.DbContexts.DbRepositories
{
    public sealed class DbUpdating<TEntity> : IDbing<TEntity> where TEntity : IDbEntity
    {
        private readonly IList<IDbEntity> _includedItems;

        public IDbRepositoryFactory DbFactory { get; }
        public TEntity Entity { get; }

        internal DbUpdating(IDbRepositoryFactory dbFactory, TEntity entity)
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

        public IDbing<TEntity> Includes<T>(Func<TEntity, ICollection<T>> selector) where T : class, IDbEntity
        {
            var ens = selector.Invoke(Entity);
            if (ens == null) return this;

            foreach (var en in ens)
                AddOrUpdate(en);

            return this;
        }

#if NETSTANDARD2_0 || NETSTANDARD1_6
        private async Task AddOrUpdateAsync<T>(T entity) where T : class, IDbEntity
        {
            if (_includedItems.Contains(entity)) return;
            _includedItems.Add(entity);

            var repo = DbFactory.For<T>();

            if (repo.IsExisted(entity.GetKeys()))
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

        public async Task<IDbing<TEntity>> IncludesAsync<T>(Func<TEntity, ICollection<T>> selector) where T : class, IDbEntity
        {
            var ens = selector.Invoke(Entity);
            if (ens == null) return this;

            await Task.WhenAll(ens.Select(AddOrUpdateAsync).ToArray());

            return this;
        }
#endif

    }
}
