#region using

using HBD.EntityFramework.DbContexts.DbEntities;
using HBD.Framework.Attributes;
using System.Threading.Tasks;

#endregion using

namespace HBD.EntityFramework.DbContexts.Interfaces
{
    /// <summary>
    ///     The RepositoryFactory for a particular entity.
    /// </summary>
    public interface IDbRepoFactory : IDbReadOnlyRepoFactory
    {
        IDbRepo<TEntity> For<TEntity>() where TEntity : class, IDbEntity;

        void EnsureDbCreated();

#if NETSTANDARD2_0 || NETSTANDARD1_6

        int Save([NotNull]string userName, bool acceptAllChangesOnSuccess = true);

        Task<int> SaveAsync([NotNull]string userName, bool acceptAllChangesOnSuccess = true);

#else
        int Save([NotNull]string userName);

        Task<int> SaveAsync([NotNull]string userName);

        Task<int> SaveAsync([NotNull]string userName, System.Threading.CancellationToken cancellationToken);
#endif
    }
}