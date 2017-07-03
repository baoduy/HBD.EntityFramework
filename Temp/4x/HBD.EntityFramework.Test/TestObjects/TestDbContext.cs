using HBD.EntityFramework.DbContexts;
using System.Data.Entity;

namespace HBD.EntityFramework.Test.TestObjects
{
    public sealed class TestDbContext : AuditTrailDbContext
    {
        static TestDbContext() { Database.SetInitializer(new CreateAuditTrailDbIfNotExists<TestDbContext>()); }

        public TestDbContext() : base("ConnectionString") { }
    }
}
