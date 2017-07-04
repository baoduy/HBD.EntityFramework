using HBD.EntityFramework.DbContexts.DbEntities;
using HBD.EntityFramework.DbContexts.DbRepositories;
using Microsoft.EntityFrameworkCore;

namespace HBD.Framework.St.Tests.TestObjs
{
    public class TestRepository<TEntity> : DbRepository<TEntity> where TEntity : class, IDbEntity<int, string>
    {
        public TestRepository(DbRepositoryFactory factory, DbContext context) : base(factory, context)
        {
        }
    }
}