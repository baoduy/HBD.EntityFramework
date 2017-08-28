using HBD.EntityFramework.Aggregates;
using HBD.EntityFramework.Core;
using HBD.Framework.Attributes;
using System.Collections.Generic;

namespace HBD.EntityFramework.Repositories
{
    public interface IDataService
    {
    }

    public interface IDataService<TEntity, in TKey> : IDataService where TEntity : Aggregate<TKey>
    {
        TEntity GetById([NotNull]TKey key);

        IPagable<TEntity> GetPage(int pageIndex, int pageSize);

        TEntity AddOrUpdate(TEntity entity);

        bool Delete(TEntity entity);

        IReadOnlyCollection<TEntity> All();
    }
}