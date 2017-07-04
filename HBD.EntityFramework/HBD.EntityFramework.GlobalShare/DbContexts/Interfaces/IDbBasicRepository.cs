#region using

using HBD.EntityFramework.DbContexts.DbEntities;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

#endregion using

namespace HBD.EntityFramework.DbContexts.Interfaces
{
    /// <summary>
    /// The Basic Repository for DbContext in-case you don't want to use the DbEntity base class.
    /// This is the wrapper of DbContext so that no need to referent the EntityFramework into every project instead manage the EntityFramework reference in a single project only.
    /// </summary>
    public interface IDbBasicRepository<TEntity> : IDbRepository where TEntity : IDbEntity
    {
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

        bool IsExisted(TEntity item);

        bool IsExisted(params object[] keyValues);

        void Add(TEntity item);

        Task AddAsync(TEntity item);

        void Update(TEntity item);

        Task UpdateAsync(TEntity item);

        void Delete(TEntity item);

        void DeleteByKey(params object[] keyValues);

        void DeleteAll();

        Task DeleteByKeyAsync(params object[] keyValues);

        void DeleteWhere(Expression<Func<TEntity, bool>> predicate);
    }
}