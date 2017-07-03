#region using

using System;
using System.Threading.Tasks;
using HBDS.EntityFramework.Base;

#endregion

namespace HBDS.EntityFramework.Repositories
{
    /// <summary>
    ///     The RepositoryFactory for a particular entity.
    /// </summary>
    public interface IRepositoryFactory : IDisposable
    {
        IRepository<TEntity> RepositoryFor<TEntity>() where TEntity : class, IEntity;
        int Save(bool acceptAllChangesOnSuccess = true);
        Task<int> SaveAsync(bool acceptAllChangesOnSuccess = true);
    }
}