using HBD.EntityFramework.DbContexts.DbRepositories;
using System.Data.Entity;
using HBD.EntityFramework.DbContexts.Interfaces;

namespace HBD.Framework.St.Tests.TestObjs
{
    public class TestFactory : DbRepoFactory
    {
        public TestFactory(DbContext context, bool autoDisposeDbContext = false) : base(context, autoDisposeDbContext)
        {
        }

        protected override IDbRepo<TEntity> CreateRepo<TEntity>()
            => new TestRepository<TEntity>(this, this.DbContext);
    }
}