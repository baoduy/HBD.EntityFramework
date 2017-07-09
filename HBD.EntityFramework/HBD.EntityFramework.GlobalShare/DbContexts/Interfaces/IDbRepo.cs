#region using

using HBD.EntityFramework.Core;
using HBD.EntityFramework.DbContexts.DbEntities;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

#endregion using

namespace HBD.EntityFramework.DbContexts.Interfaces
{
    public interface IDbRepo { }
    /// <summary>
    /// The Basic Repository for DbContext in-case you don't want to use the DbEntity base class.
    /// This is the wrapper of DbContext so that no need to referent the EntityFramework into every project instead manage the EntityFramework reference in a single project only.
    /// </summary>
    public interface IDbRepo<TEntity> : IDbReadOnlyRepo<TEntity> where TEntity : IDbEntity
    {
        #region Write Actions
        void Add(TEntity item);

        Task AddAsync(TEntity item);

        IDbUpdating<TEntity> Update(TEntity item);

        Task<IDbUpdating<TEntity>> UpdateAsync(TEntity item);

        void Delete(TEntity item);

        void DeleteByKey(params object[] keyValues);

        void DeleteAll();

        Task DeleteByKeyAsync(params object[] keyValues);

        void DeleteWhere(Expression<Func<TEntity, bool>> predicate);
        #endregion
    }
}