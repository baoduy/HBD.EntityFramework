#region using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HBD.EntityFramework.DbContexts.DbEntities;

#endregion using

namespace HBD.EntityFramework.Core
{
    public interface IDbRepo { }
    /// <summary>
    /// The Basic Repository for DbContext in-case you don't want to use the DbEntity base class.
    /// This is the wrapper of DbContext so that no need to referent the EntityFramework into every project instead manage the EntityFramework reference in a single project only.
    /// </summary>
    public interface IDbRepo<TEntity> : IDbRepo where TEntity :class, IDbEntity
    {
        #region ReadOnly Actions
        IEnumerable<EntityStatus<TEntity>> GetChangingEntities();

        IQueryable<TEntity> AsQueryable(Expression<Func<TEntity, bool>> predicate = null);

        IOrderedQueryable<TEntity> AsOrderable(string orderBy,
            OrderDirection direction = OrderDirection.Asscending, Expression<Func<TEntity, bool>> predicate = null);

        TEntity Find(params object[] keyValues);

        Task<TEntity> FindAsync(params object[] keyValues);

        IPagable<TEntity> Page<TKey>(int pageIndex, int pageSize,
            Expression<Func<TEntity, TKey>> orderBy, OrderDirection direction = OrderDirection.Asscending,
            Expression<Func<TEntity, bool>> predicate = null);

        Task<IPagable<TEntity>> PageAsync<TKey>(int pageIndex, int pageSize,
            Expression<Func<TEntity, TKey>> orderBy, OrderDirection direction = OrderDirection.Asscending,
            Expression<Func<TEntity, bool>> predicate = null);

        bool IsExisted(params object[] keyValues);
        Task<bool> IsExistedAsync(params object[] keyValues);
        #endregion

        #region Write Actions
        void Add(TEntity item);

        Task AddAsync(TEntity item);

        IDbing<TEntity> Update(TEntity item);

        Task<IDbing<TEntity>> UpdateAsync(TEntity item);

        IDbing<TEntity> Delete(TEntity item);

        Task<IDbing<TEntity>> DeleteAsync(TEntity item);

        void DeleteByKey(params object[] keyValues);
        Task DeleteByKeyAsync(params object[] keyValues);

        int DeleteAll();
        Task<int> DeleteAllAsync();

        int Delete(Expression<Func<TEntity, bool>> predicate);
        Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate);
        #endregion
    }
}