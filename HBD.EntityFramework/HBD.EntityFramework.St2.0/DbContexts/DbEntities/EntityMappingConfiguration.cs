#if NETSTANDARD2_0
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HBD.EntityFramework.DbContexts.DbEntities
{
    public abstract class EntityMappingConfiguration<TEntity> : IEntityMappingConfiguration<TEntity>
        where TEntity : DbEntity
    {
        public virtual void Map(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Property(a => a.CreatedBy);
            builder.Property(a => a.CreatedOn);
            builder.Property(a => a.RowVersion)
                .IsConcurrencyToken()
                .IsRowVersion();
            builder.Property(a => a.UpdatedBy);
            builder.Property(a => a.UpdatedOn);
        }
    }
}
#endif