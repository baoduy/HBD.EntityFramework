using HBD.EntityFramework.DbContexts.DbEntities;
using HBD.EntityFramework.Sample.DbEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HBD.EntityFramework.Sample.DbContexts.Mappers
{
    internal class EmailMapper : EntityMappingConfiguration<EmailAddessDb>
    {
        public override void Map(EntityTypeBuilder<EmailAddessDb> builder)
        {
            base.Map(builder);

            builder.ToTable("Emails");
            builder.Property(m => m.Name).IsRequired();
            builder.Property(m => m.Email).IsRequired();
        }
    }
}