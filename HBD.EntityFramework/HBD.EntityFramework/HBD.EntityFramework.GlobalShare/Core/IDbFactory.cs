#region using

using HBD.Framework.Attributes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HBD.EntityFramework.DbContexts.DbEntities;

#endregion using

namespace HBD.EntityFramework.Core
{
    /// <inheritdoc />
    /// <summary>
    ///     The RepositoryFactory for a particular entity.
    /// </summary>
    public interface IDbFactory:IDisposable
    {
        IDbRepo<TEntity> For<TEntity>() where TEntity : class, IDbEntity;

        IEnumerable<EntityStatus<TEntity>> GetChangingEntities<TEntity>()where TEntity : class, IDbEntity;

        /// <summary>
        /// Ensure the Database is created and ready for use.
        /// </summary>
        void EnsureDbCreated();

        /// <summary>
        /// Save the changes to Db.
        /// </summary>
        /// <param name="byUserNameOrId">The user name or id. The value type must be according to CreatedBy and UpdatedBy value type.</param>
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
    }
}