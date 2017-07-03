using HBD.EntityFramework.AuditTrail.Entities;
using System.Data.Entity.ModelConfiguration;

namespace HBD.EntityFramework.AuditTrail.Maps
{
    internal class AuditTrailActionTypeMap : EntityTypeConfiguration<AuditTrailActionType>
    {
        public AuditTrailActionTypeMap()
        {
            //Primary Key
            //this.HasKey(t => t.Id);

            //this.Property(t => t.Name)
            //    .IsRequired();

            // Table & Column Mappings
            this.ToTable("ActionType", "Audit");
        }
    }
}