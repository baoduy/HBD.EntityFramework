using HBD.EntityFramework.DbContexts.DbEntities;
using HBD.EntityFramework.DbContexts.DbRepositories;
using System.Data.Entity;

namespace HBD.Framework.St.Tests.TestObjs
{
    public class TestRepository<TEntity> : DbRepo<TEntity> where TEntity :class, IDbEntity
    {
        public TestRepository(DbRepoFactory factory, DbContext context) : base(factory, context)
        {
        }
    }
}