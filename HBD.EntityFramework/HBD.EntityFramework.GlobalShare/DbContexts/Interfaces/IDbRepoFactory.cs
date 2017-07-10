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
        /// <summary>
        /// Save the changes to Db.
        /// </summary>
        /// <param name="byUserNameOrId">The user name or id. The value type must be according to CreatedBy and UpdatedBy value type.</param>
        /// <param name="acceptAllChangesOnSuccess"></param>
        /// <returns>The effected items in Db.</returns>
        /// <exception cref="System.InvalidCastException">Thrown when the userName is not able to assign to CreatedBy or UpdatedBy</exception>
        int Save([NotNull]object byUserNameOrId, bool acceptAllChangesOnSuccess = true);

        /// <summary>
        /// Save the changes to Db.
        /// </summary>
        /// <param name="byUserNameOrId">The user name or id. The value type must be according to CreatedBy and UpdatedBy value type.</param>
        /// <param name="acceptAllChangesOnSuccess"></param>
        /// <returns>The effected items in Db.</returns>
        /// <exception cref="System.InvalidCastException">Thrown when the userName is not able to assign to CreatedBy or UpdatedBy</exception>
        Task<int> SaveAsync([NotNull]object byUserNameOrId, bool acceptAllChangesOnSuccess = true);

#else
         /// <summary>
        /// Save the changes to Db.
        /// </summary>
        /// <param name="userName">The user name or id. The value type must be according to CreatedBy and UpdatedBy value type.</param>
        /// <returns>The effected items in Db.</returns>
        /// <exception cref="System.InvalidCastException">Thrown when the userName is not able to assign to CreatedBy or UpdatedBy</exception>
        int Save([NotNull]object byUserNameOrId);

         /// <summary>
        /// Save the changes to Db.
        /// </summary>
        /// <param name="byUserNameOrId">The user name or id. The value type must be according to CreatedBy and UpdatedBy value type.</param>
        /// <returns>The effected items in Db.</returns>
        /// <exception cref="System.InvalidCastException">Thrown when the userName is not able to assign to CreatedBy or UpdatedBy</exception>
        Task<int> SaveAsync([NotNull]object byUserNameOrId);

         /// <summary>
        /// Save the changes to Db.
        /// </summary>
        /// <param name="byUserNameOrId">The user name or id. The value type must be according to CreatedBy and UpdatedBy value type.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The effected items in Db.</returns>
        /// <exception cref="System.InvalidCastException">Thrown when the userName is not able to assign to CreatedBy or UpdatedBy</exception>
        Task<int> SaveAsync([NotNull]object byUserNameOrId, System.Threading.CancellationToken cancellationToken);
#endif
    }
}