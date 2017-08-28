using HBD.EntityFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HBD.EntityFramework.DbContexts.DbRepositories
{
    public sealed class DbDeleting<TEntity> : IDbing<TEntity> where TEntity : IDbEntity
    {
        private readonly IList<IDbEntity> _includedItems;

        public IDbRepositoryFactory DbFactory { get; }
        public TEntity Entity { get; }

        internal DbDeleting(IDbRepositoryFactory dbFactory, TEntity entity)
        {
            DbFactory = dbFactory;
            Entity = entity;
            _includedItems = new List<IDbEntity>();
        }

        private void Delete<T>(T entity) where T : class, IDbEntity
        {
            if (_includedItems.Contains(entity)) return;
            _includedItems.Add(entity);

            DbFactory.For<T>().Delete(entity);
        }

        public IDbing<TEntity> Include<T>(Func<TEntity, T> selector) where T : class, IDbEntity
        {
            var en = selector.Invoke(Entity);
            if (en == null) return this;

            Delete(en);

            return this;
        }

        public IDbing<TEntity> Includes<T>(Func<TEntity, ICollection<T>> selector) where T : class, IDbEntity
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

            await DbFactory.For<T>().DeleteAsync(entity);
        }

        public async Task<IDbing<TEntity>> IncludeAsync<T>(Func<TEntity, T> selector) where T : class, IDbEntity
        {
            var en = selector.Invoke(Entity);
            if (en == null) return this;

            await DeleteAsync(en);

            return this;
        }

        public async Task<IDbing<TEntity>> IncludesAsync<T>(Func<TEntity, ICollection<T>> selector) where T : class, IDbEntity
        {
            var ens = selector.Invoke(Entity);
            if (ens == null) return this;

            await Task.WhenAll(ens.Select(DeleteAsync).ToArray());

            return this;
        }
#endif

    }
}
