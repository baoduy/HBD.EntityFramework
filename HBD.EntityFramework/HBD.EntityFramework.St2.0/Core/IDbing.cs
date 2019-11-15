using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HBD.EntityFramework.Core
{
    public interface IDbing<TEntity> where TEntity : IDbEntity
    {
        IDbing<TEntity> Includes<T>(Func<TEntity, IEnumerable<T>> selector) where T : class, IDbEntity;
        IDbing<TEntity> Include<T>(Func<TEntity, T> selector) where T : class, IDbEntity;

#if NETSTANDARD2_0
        Task<IDbing<TEntity>> IncludesAsync<T>(Func<TEntity, IEnumerable<T>> selector) where T : class, IDbEntity;
        Task<IDbing<TEntity>> IncludeAsync<T>(Func<TEntity, T> selector) where T : class, IDbEntity;
#endif
    }
}
