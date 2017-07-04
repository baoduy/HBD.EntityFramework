using HBD.EntityFramework.DbContexts.DbRepositories;
using System.Data.Entity;
using HBD.EntityFramework.DbContexts.Interfaces;

namespace HBD.Framework.St.Tests.TestObjs
{
    public class TestFactory : DbRepositoryFactory
    {
        public TestFactory(DbContext context, bool autoDisposeDbContext = false) : base(context, autoDisposeDbContext)
        {
        }

        protected override IDbRepository<TEntity, int, string> CreateRepository<TEntity>()
            => new TestRepository<TEntity>(this, this.DbContext);
    }
}