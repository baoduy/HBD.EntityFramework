#region using

using System;
using System.Threading.Tasks;
using HBD.EntityFramework.Core;

#endregion

namespace HBD.EntityFramework.Repositories
{
    /// <summary>
    ///     The RepositoryFactory for a particular entity.
    /// </summary>
    public interface IRepositoryFactory : IDisposable
    {
        IRepository<TEntity> RepositoryFor<TEntity>() where TEntity : class, IEntity;

#if NETSTANDARD2_0 || NETSTANDARD1_6
        int Save(bool acceptAllChangesOnSuccess = true);

        Task<int> SaveAsync(bool acceptAllChangesOnSuccess = true);
#else
        int Save();

        Task<int> SaveAsync();

        Task<int> SaveAsync(System.Threading.CancellationToken  cancellationToken);
#endif
    }
}