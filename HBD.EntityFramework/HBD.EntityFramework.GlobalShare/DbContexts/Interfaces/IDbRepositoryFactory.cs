#region using

using HBD.EntityFramework.DbContexts.DbEntities;
using System;
using System.Threading.Tasks;

#endregion using

namespace HBD.EntityFramework.DbContexts.Interfaces
{
    /// <summary>
    ///     The RepositoryFactory for a particular entity.
    /// </summary>
    public interface IDbRepositoryFactory : IDisposable
    {
        IDbRepository<TEntity, int, string> For<TEntity>() where TEntity :class, IDbEntity<int, string>;

        void EnsureDbCreated();

#if NETSTANDARD2_0 || NETSTANDARD1_6

        int Save(bool acceptAllChangesOnSuccess = true);

        Task<int> SaveAsync(bool acceptAllChangesOnSuccess = true);

#else
        int Save();

        Task<int> SaveAsync();

        Task<int> SaveAsync(System.Threading.CancellationToken cancellationToken);
#endif
    }
}