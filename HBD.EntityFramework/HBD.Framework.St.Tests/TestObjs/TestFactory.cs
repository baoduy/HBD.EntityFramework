using HBD.EntityFramework.DbContexts.DbRepositories;
using HBD.EntityFramework.DbContexts.Interfaces;
using Microsoft.EntityFrameworkCore;

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