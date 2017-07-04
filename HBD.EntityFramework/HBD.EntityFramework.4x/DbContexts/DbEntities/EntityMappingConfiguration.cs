using System.Data.Entity.ModelConfiguration;

namespace HBD.EntityFramework.DbContexts.DbEntities
{
    public abstract class EntityMappingConfiguration<TEntity> : IEntityMappingConfiguration<TEntity> where TEntity : DbEntity
    {
        public virtual void Map(EntityTypeConfiguration<TEntity> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id).IsRequired()
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            builder.Property(a => a.CreatedBy);
            builder.Property(a => a.CreatedTime);
            builder.Property(a => a.RowVersion)
                .IsConcurrencyToken(true)
                .IsRowVersion();
            builder.Property(a => a.UpdatedBy);
            builder.Property(a => a.UpdatedTime);
        }
    }
}
