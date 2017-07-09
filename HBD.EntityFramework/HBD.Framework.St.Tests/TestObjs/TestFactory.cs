using HBD.EntityFramework.DbContexts.DbRepositories;
using HBD.EntityFramework.DbContexts.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HBD.Framework.St.Tests.TestObjs
{
    public class TestFactory : DbRepoFactory
    {
        public TestFactory(DbContext context, bool autoDisposeDbContext = false) : base(context, autoDisposeDbContext)
        {
        }

        protected override IDbRepo<TEntity> CreateRepo<TEntity>()
            => new TestBasicRepository<TEntity>(this, this.DbContext);
    }
}