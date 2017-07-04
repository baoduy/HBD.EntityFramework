using HBD.EntityFramework.Core;
using HBD.EntityFramework.DbContexts.DbEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HBD.EntityFramework.DbContexts.DbRepositories
{
    public sealed class DbEntityUpdating<TEntity> : IDbEntityUpdating<TEntity> where TEntity : IDbEntity<int, string>
    {
        private readonly IList<IDbEntity> _includedItems;

        public DbRepositoryFactory DbFactory { get; }
        public TEntity Entity { get; }
        public string Updator { get; }

        internal DbEntityUpdating(DbRepositoryFactory dbFactory, TEntity entity, string updator)
        {
            DbFactory = dbFactory;
            Entity = entity;
            Updator = updator;
            _includedItems = new List<IDbEntity>();
        }

        private void AddOrUpdate<T>(T entity) where T : class, IDbEntity<int, string>
        {
            if (_includedItems.Contains(entity)) return;
            _includedItems.Add(entity);

            var repo = DbFactory.For<T>();

            if (repo.IsExisted(entity.Id))
                repo.Update(entity, this.Updator);
            else
                repo.Add(entity, this.Updator);
        }

        public IDbUpdating<TEntity, int, string> Include<T>(Func<TEntity, T> selector) where T : class, IDbEntity<int, string>
        {
            var en = selector.Invoke(this.Entity);
            if (en == null) return this;

            AddOrUpdate(en);

            return this;
        }

        public IDbUpdating<TEntity, int, string> Includes<T>(Func<TEntity, ICollection<T>> selector) where T : class, IDbEntity<int, string>
        {
            var ens = selector.Invoke(this.Entity);
            if (ens == null) return this;

            foreach (var en in ens)
                AddOrUpdate(en);

            return this;
        }

#if NETSTANDARD2_0 || NETSTANDARD1_6
        private async Task AddOrUpdateAsync<T>(T entity) where T : class, IDbEntity<int, string>
        {
            if (_includedItems.Contains(entity)) return;
            _includedItems.Add(entity);

            var repo = DbFactory.For<T>();

            if (repo.IsExisted(entity.Id))
                await repo.UpdateAsync(entity, this.Updator);
            else
                await repo.AddAsync(entity, this.Updator);
        }

        public async Task<IDbUpdating<TEntity, int, string>> IncludeAsync<T>(Func<TEntity, T> selector) where T : class, IDbEntity<int, string>
        {
            var en = selector.Invoke(this.Entity);
            if (en == null) return this;

            await AddOrUpdateAsync(en);

            return this;
        }

        public async Task<IDbUpdating<TEntity, int, string>> IncludesAsync<T>(Func<TEntity, ICollection<T>> selector) where T : class, IDbEntity<int, string>
        {
            var ens = selector.Invoke(this.Entity);
            if (ens == null) return this;

            await Task.WhenAll(ens.Select(AddOrUpdateAsync).ToArray());

            return this;
        }
#endif

    }
}
