#region using

using HBD.EntityFramework.Core;

#if NETSTANDARD2_0 || NETSTANDARD1_6
using Microsoft.EntityFrameworkCore;
#else
using System.Data.Entity;
#endif

#endregion

namespace HBD.EntityFramework.Repositories
{
    public class EntityRepositoryFactory : RepositoryFactory, IRepositoryFactory
    {
        public EntityRepositoryFactory(DbContext context, bool autoDisposeDbContext = false)
            : base(context, autoDisposeDbContext)
        {
        }

        public IRepository<TEntity> RepositoryFor<TEntity>() where TEntity : class, IEntity
            => Cacher.GetOrAdd(typeof(TEntity), k => CreateRepository<TEntity>()) as IRepository<TEntity>;

        protected virtual IRepository<TEntity> CreateRepository<TEntity>() where TEntity : class, IEntity
            => new EntityRepository<TEntity>(this, Context);
    }
}