using System.Data.Entity.ModelConfiguration;

namespace HBD.EntityFramework.DbContexts.DbEntities
{
    public interface IEntityMappingConfiguration<TEntity> where TEntity : class
    {
        void Map(EntityTypeConfiguration<TEntity> builder);
    }
}
