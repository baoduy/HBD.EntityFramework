using System.Data.Common;
using System.Data.Entity;

namespace HBD.Framework.St.Tests.TestObjs
{
    public class TestDbContext : DbContext
    {
        public TestDbContext(DbConnection connection) : base(connection, false)
        { }

        public TestDbContext(string connectionstring) : base(connectionstring)
        { }

        public TestDbContext() : base() { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Person>()
              .ToTable("Person");
        }
    }
}
