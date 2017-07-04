using HBD.EntityFramework.DbContexts.BasicRepositories;
using HBD.EntityFramework.DbContexts.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HBD.Framework.St.Tests.TestObjs
{
    public class TestBasicFactory : DbBasicRepositoryFactory
    {
        public TestBasicFactory(DbContext context, bool autoDisposeDbContext = false)
            : base(context, autoDisposeDbContext)
        {
        }

        protected override IDbBasicRepository<TEntity> CreateRepository<TEntity>()
            => new TestBasicRepository<TEntity>(this, this.Context);
    }
}