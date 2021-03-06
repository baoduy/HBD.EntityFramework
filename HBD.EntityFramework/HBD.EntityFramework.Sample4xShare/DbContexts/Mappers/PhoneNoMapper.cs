﻿using HBD.EntityFramework.DbContexts.DbEntities;
using HBD.EntityFramework.Sample.DbEntities;
using System.Data.Entity.ModelConfiguration;

namespace HBD.EntityFramework.Sample.DbContexts.Mappers
{
    internal class PhoneNoMapper : EntityMappingConfiguration<PhoneNumberDb>
    {
        public override void Map(EntityTypeConfiguration<PhoneNumberDb> builder)
        {
            base.Map(builder);

            builder.ToTable("PhoneNumbers");
            builder.Property(m => m.Name).IsRequired();
            builder.Property(m => m.PhoneNo).IsRequired();
        }
    }
}