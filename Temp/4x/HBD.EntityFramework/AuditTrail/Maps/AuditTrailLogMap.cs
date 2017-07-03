using HBD.EntityFramework.AuditTrail.Entities;
using System.Data.Entity.ModelConfiguration;

namespace HBD.EntityFramework.AuditTrail.Maps
{
    public class AuditTrailLogMap : EntityTypeConfiguration<AuditTrailLog>
    {
        public AuditTrailLogMap()
        {
            //Primary Key
            //this.HasKey(t => t.Id);

            // Table & Column Mappings
            this.ToTable("Log", "Audit");

            this.HasMany(x => x.Details)
                .WithRequired(x => x.Log)
                .HasForeignKey(x => x.AudiTrailId);
        }
    }
}