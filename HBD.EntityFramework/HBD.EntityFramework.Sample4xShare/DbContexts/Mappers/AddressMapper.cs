using HBD.EntityFramework.DbContexts.DbEntities;
using HBD.EntityFramework.Sample.DbEntities;
using System.Data.Entity.ModelConfiguration;

namespace HBD.EntityFramework.Sample.DbContexts.Mappers
{
    internal class AddressMapper : EntityMappingConfiguration<AddressDb>
    {
        public override void Map(EntityTypeConfiguration<AddressDb> builder)
        {
            base.Map(builder);

            builder.ToTable("Addresses");
            builder.Property(m => m.BlockNo);
            builder.Property(m => m.City);
            builder.Property(m => m.Country);
            builder.Property(m => m.PostalCode);
            builder.Property(m => m.Street);
        }
    }
}