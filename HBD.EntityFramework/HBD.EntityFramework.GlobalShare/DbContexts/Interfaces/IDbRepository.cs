#region using

using HBD.EntityFramework.Core;
using HBD.EntityFramework.DbContexts.DbEntities;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

#endregion using

namespace HBD.EntityFramework.DbContexts.Interfaces
{
    /// <summary>
    /// The Repository for DbContext.
    /// This is the wrapper of DbContext so that no need to referent the EntityFramework into every project instead manage the EntityFramework reference in a single project only.
    /// </summary>
    public interface IDbRepository
    {
    }

    /// <summary>
    /// The Repository for DbContext.
    /// This is the wrapper of DbContext so that no need to referent the EntityFramework into every project instead manage the EntityFramework reference in a single project only.
    /// </summary>
    public interface IDbRepository<TEntity, TKey, TAuditKey> : IDbRepository where TEntity : IDbEntity<TKey, TAuditKey>
    {
        IQueryable<TEntity> AsQueryable(Expression<Func<TEntity, bool>> predicate = null);

        IOrderedQueryable<TEntity> AsOrderable(string orderBy,
            OrderDirection direction = OrderDirection.Asscending, Expression<Func<TEntity, bool>> predicate = null);

        TEntity Find(TKey keyValues);

        Task<TEntity> FindAsync(TKey keyValues);

        IPagable<TEntity> Page(int pageIndex, int pageSize,
            Expression<Func<TEntity, TKey>> orderBy, OrderDirection direction = OrderDirection.Asscending,
            Expression<Func<TEntity, bool>> predicate = null);

        Task<IPagable<TEntity>> PageAsync(int pageIndex, int pageSize,
            Expression<Func<TEntity, TKey>> orderBy, OrderDirection direction = OrderDirection.Asscending,
            Expression<Func<TEntity, bool>> predicate = null);

        bool IsExisted(TEntity item);

        bool IsExisted(TKey keyValues);

        void Add(TEntity item, TAuditKey creatingUser);

        Task AddAsync(TEntity item, TAuditKey userName);

        IDbUpdating<TEntity, TKey, TAuditKey> Update(TEntity item, TAuditKey updatingUser);

        Task<IDbUpdating<TEntity, TKey, TAuditKey>> UpdateAsync(TEntity item, TAuditKey updatingUser);

        void Delete(TEntity item, TAuditKey deletingUser);

        void DeleteByKey(TKey keyValues);

        void DeleteAll(TAuditKey deletingUser);

        Task DeleteByKeyAsync(TKey keyValues);

        void DeleteWhere(Expression<Func<TEntity, bool>> predicate, TAuditKey deletingUser);
    }
}