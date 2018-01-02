#region using

using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

#endregion using

namespace HBD.EntityFramework.Core
{
    public interface IDbRepository { }
    /// <summary>
    /// The Basic Repository for DbContext in-case you don't want to use the DbEntity base class.
    /// This is the wrapper of DbContext so that no need to referent the EntityFramework into every project instead manage the EntityFramework reference in a single project only.
    /// </summary>
    public interface IDbRepository<TEntity> : IDbRoRepository<TEntity> where TEntity : IDbEntity
    {
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