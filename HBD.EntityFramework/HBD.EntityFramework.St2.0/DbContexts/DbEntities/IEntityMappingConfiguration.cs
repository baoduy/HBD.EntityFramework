#if NETSTANDARD2_0
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HBD.EntityFramework.DbContexts.DbEntities
{
    public interface IEntityMappingConfiguration<TEntity> where TEntity : class
    {
        void Map(EntityTypeBuilder<TEntity> builder);
    }
}
#endif