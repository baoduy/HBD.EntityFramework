using HBD.EntityFramework.DbContexts.DbEntities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HBD.EntityFramework.Core
{
    public interface IDbUpdating<TEntity, TKey, TAuditKey> where TEntity : IDbEntity<TKey, TAuditKey>
    {
        IDbUpdating<TEntity, TKey, TAuditKey> Includes<T>(Func<TEntity, ICollection<T>> selector) where T : class, IDbEntity<TKey, TAuditKey>;
        IDbUpdating<TEntity, TKey, TAuditKey> Include<T>(Func<TEntity, T> selector) where T : class, IDbEntity<TKey, TAuditKey>;

#if NETSTANDARD2_0 || NETSTANDARD1_6
        Task<IDbUpdating<TEntity, TKey, TAuditKey>> IncludesAsync<T>(Func<TEntity, ICollection<T>> selector) where T : class, IDbEntity<TKey, TAuditKey>;
        Task<IDbUpdating<TEntity, TKey, TAuditKey>> IncludeAsync<T>(Func<TEntity, T> selector) where T : class, IDbEntity<TKey, TAuditKey>;
#endif
    }

    public interface IDbEntityUpdating<TEntity>: IDbUpdating<TEntity, int, string> where TEntity : IDbEntity<int, string>
    {
       
    }
}
