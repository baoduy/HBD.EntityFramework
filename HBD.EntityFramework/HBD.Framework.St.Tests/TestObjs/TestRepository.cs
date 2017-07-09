using HBD.EntityFramework.DbContexts.DbEntities;
using HBD.EntityFramework.DbContexts.DbRepositories;
using HBD.EntityFramework.DbContexts.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HBD.Framework.St.Tests.TestObjs
{
    public class TestRepository<TEntity> : DbRepo<TEntity> where TEntity : class, IDbEntity<int, string>
    {
        public TestRepository(IDbRepoFactory factory, DbContext context) : base(factory, context)
        {
        }
    }
}