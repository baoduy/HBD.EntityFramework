using HBD.EntityFramework.Base;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace HBD.EntityFramework.DbContexts
{
    /// <summary>
    /// This ExtenedDbContext will apply the values for properties: CreatedBy, CreatedTime. UpdatedBy and UpdatedTime for all entities which delivered from HBD.EntityFramework.Entities.IEntity.
    /// </summary>
    public class ExtenedDbContext : DbContext, IDbContext
    {
        public ExtenedDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        #region SaveChanges

        public override int SaveChanges()
        {
            this.ApplyAuditProperties();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(System.Threading.CancellationToken cancellationToken)
        {
            this.ApplyAuditProperties();
            return base.SaveChangesAsync(cancellationToken);
        }

        #endregion SaveChanges

        /// <summary>
        /// Get all DbEntityEntry<IEntityBase/> in ChangeTracker and call ApplyEntityProperties(DbEntityEntry<IEntityBase/> entity) method
        /// </summary>
        protected virtual void ApplyAuditProperties()
        {
            foreach (var entity in ChangeTracker.Entries<IEntity>().Where(e => e.Entity.UpdateAuditPropertyMode == UpdateAuditPropertyMode.Auto))
                this.ApplyAuditProperties(entity);
        }

        /// <summary>
        /// Apply values for Extend properties.
        /// </summary>
        /// <param name="entity"></param>
        protected virtual void ApplyAuditProperties(DbEntityEntry<IEntity> entity) => Common.CommonHelper.ApplyAuditProperties(entity.Entity, entity.State);
    }
}