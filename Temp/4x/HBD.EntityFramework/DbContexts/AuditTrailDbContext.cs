using HBD.EntityFramework.AuditTrail.Entities;
using HBD.EntityFramework.AuditTrail.Maps;
using HBD.EntityFramework.Base;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace HBD.EntityFramework.DbContexts
{
    public class AuditTrailDbContext : ExtenedDbContext
    {
        static AuditTrailDbContext()
        {
            Database.SetInitializer(new CreateAuditTrailDbIfNotExists<AuditTrailDbContext>());
        }

        public AuditTrailDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            this.AuditTrailManager = new AuditTrailManager(this);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new AuditTrailLogMap());
            modelBuilder.Configurations.Add(new AuditTrailDetailsMap());
            modelBuilder.Configurations.Add(new AuditTrailActionTypeMap());
        }

        protected override void ApplyAuditProperties(DbEntityEntry<IEntity> entity)
        {
            base.ApplyAuditProperties(entity);

            if (!Common.CommonHelper.IsTrackable(entity)) return;
            var log = Common.CommonHelper.GetAuditLog(entity, this);
            this.AuditTrailLogs.Add(log);
        }

        internal virtual DbSet<AuditTrailLog> AuditTrailLogs => this.Set<AuditTrailLog>();
        internal virtual DbSet<AuditTrailDetails> AuditTrailDetails => this.Set<AuditTrailDetails>();
        internal virtual DbSet<AuditTrailActionType> AuditTrailActionTypes => this.Set<AuditTrailActionType>();
        public AuditTrailManager AuditTrailManager { get; }
    }
}