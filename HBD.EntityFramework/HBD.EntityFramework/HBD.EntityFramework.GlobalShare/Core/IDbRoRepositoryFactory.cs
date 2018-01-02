using System;

namespace HBD.EntityFramework.Core
{
    /// <summary>
    /// The Read Only Repository Factory.
    /// </summary>
    public interface IDbRoRepositoryFactory: IDisposable
    {
        IDbRoRepository<TEntity> ReadOnlyFor<TEntity>() where TEntity : class, IDbEntity;
    }
}
