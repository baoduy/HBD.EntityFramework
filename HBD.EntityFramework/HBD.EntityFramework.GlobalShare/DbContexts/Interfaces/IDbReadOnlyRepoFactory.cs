using HBD.EntityFramework.DbContexts.DbEntities;
using System;

namespace HBD.EntityFramework.DbContexts.Interfaces
{
    public interface IDbReadOnlyRepoFactory: IDisposable
    {
        IDbReadOnlyRepo<TEntity> ReadOnlyFor<TEntity>() where TEntity : class, IDbEntity;
    }
}
