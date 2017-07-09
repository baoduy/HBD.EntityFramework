using System;
using System.Linq;
using System.Linq.Expressions;
using HBD.EntityFramework.DbContexts.DbEntities;
using System.Threading.Tasks;

namespace HBD.EntityFramework.DbContexts.Interfaces
{
    public interface IDbReadOnlyRepo<TEntity> : IDbRepo where TEntity : IDbEntity
    {
        #region ReadOnly Actions
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
        #endregion
    }
}
