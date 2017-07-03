#region using

using HBDS.EntityFramework.Base;
using Microsoft.EntityFrameworkCore;

#endregion

namespace HBDS.EntityFramework.Repositories
{
    public class EntityRepositoryFactory : RepositoryFactoryBase, IRepositoryFactory
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