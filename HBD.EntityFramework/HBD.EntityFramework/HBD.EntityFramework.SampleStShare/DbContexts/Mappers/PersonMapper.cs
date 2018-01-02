using HBD.EntityFramework.DbContexts.DbEntities;
using HBD.EntityFramework.Sample.DbEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HBD.EntityFramework.Sample.DbContexts.Mappers
{
    internal class PersonMapper : EntityMappingConfiguration<PersonDb>
    {
        public override void Map(EntityTypeBuilder<PersonDb> builder)
        {
            base.Map(builder);

            builder.ToTable("People");

            builder.Property(m => m.FirstName);
            builder.Property(m => m.LastName);

#if NETSTANDARD2_0
            builder.HasMany(m => m.Addresses)
                .WithOne(a => a.Person)
                .HasForeignKey(a => a.PersonId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(m => m.PhoneNumbers)
                .WithOne(a => a.Person)
                .HasForeignKey(a => a.PersonId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(m => m.EmailAddresses)
                .WithOne(a => a.Person)
                .HasForeignKey(a => a.PersonId)
                .OnDelete(DeleteBehavior.Cascade);
#else
            builder.HasMany(m => m.Addresses)
             .WithOne(a => a.Person)
             .HasForeignKey(a => a.PersonId);

            builder.HasMany(m => m.PhoneNumbers)
                .WithOne(a => a.Person)
                .HasForeignKey(a => a.PersonId);

            builder.HasMany(m => m.EmailAddresses)
                .WithOne(a => a.Person)
                .HasForeignKey(a => a.PersonId);
#endif
        }
    }
}