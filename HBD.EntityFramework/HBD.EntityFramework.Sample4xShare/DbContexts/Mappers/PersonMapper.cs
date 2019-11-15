using HBD.EntityFramework.DbContexts.DbEntities;
using HBD.EntityFramework.Sample.DbEntities;
using System.Data.Entity.ModelConfiguration;

namespace HBD.EntityFramework.Sample.DbContexts.Mappers
{
    internal class PersonMapper : EntityMappingConfiguration<PersonDb>
    {
        public override void Map(EntityTypeConfiguration<PersonDb> builder)
        {
            base.Map(builder);

            builder.ToTable("People");

            builder.Property(m => m.FirstName);
            builder.Property(m => m.LastName);

            builder.HasMany(m => m.Addresses)
             .WithRequired(a => a.Person)
             .HasForeignKey(a => a.PersonId);

            builder.HasMany(m => m.PhoneNumbers)
                .WithRequired(a => a.Person)
                .HasForeignKey(a => a.PersonId);

            builder.HasMany(m => m.EmailAddresses)
                .WithRequired(a => a.Person)
                .HasForeignKey(a => a.PersonId);
        }
    }
}