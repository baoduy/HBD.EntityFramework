using HBD.EntityFramework.AuditTrail.Entities;
using System.Data.Entity.ModelConfiguration;

namespace HBD.EntityFramework.AuditTrail.Maps
{
    public class AuditTrailDetailsMap : EntityTypeConfiguration<AuditTrailDetails>
    {
        public AuditTrailDetailsMap()
        {
            //Primary Key
            //this.HasKey(t => t.Id);

            //this.Property(t => t.Name)
            //    .IsRequired();

            // Table & Column Mappings
            this.ToTable("Details", "Audit");
        }
    }
}