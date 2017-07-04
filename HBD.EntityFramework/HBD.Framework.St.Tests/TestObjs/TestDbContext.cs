using Microsoft.EntityFrameworkCore;

namespace HBD.Framework.St.Tests.TestObjs
{
    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
        { }

        public TestDbContext() : base()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var e = modelBuilder.Entity<Person>();
            e.ToTable("Person");

            e.Property(a => a.Id)
               .IsRequired()
               .ValueGeneratedOnAdd();

            e.Property(a => a.RowVersion)
                .IsConcurrencyToken(true)
                .IsRowVersion();
        }
    }
}