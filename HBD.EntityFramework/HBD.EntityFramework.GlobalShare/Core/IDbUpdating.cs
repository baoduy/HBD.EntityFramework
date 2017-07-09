using HBD.EntityFramework.DbContexts.DbEntities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HBD.EntityFramework.Core
{
    public interface IDbUpdating<TEntity> where TEntity : IDbEntity
    {
        IDbUpdating<TEntity> Includes<T>(Func<TEntity, ICollection<T>> selector) where T : class, IDbEntity;
        IDbUpdating<TEntity> Include<T>(Func<TEntity, T> selector) where T : class, IDbEntity;

#if NETSTANDARD2_0 || NETSTANDARD1_6
        Task<IDbUpdating<TEntity>> IncludesAsync<T>(Func<TEntity, ICollection<T>> selector) where T : class, IDbEntity;
        Task<IDbUpdating<TEntity>> IncludeAsync<T>(Func<TEntity, T> selector) where T : class, IDbEntity;
#endif
    }
}
