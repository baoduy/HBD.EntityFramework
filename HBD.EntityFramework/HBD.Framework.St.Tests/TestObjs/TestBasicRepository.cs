using HBD.EntityFramework.DbContexts.DbEntities;
using HBD.EntityFramework.DbContexts.DbRepositories;
using HBD.EntityFramework.DbContexts.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HBD.Framework.St.Tests.TestObjs
{
    public class TestBasicRepository<TEntity> : DbRepo<TEntity> where TEntity : class, IDbEntity
    {
        public TestBasicRepository(IDbRepoFactory factory, DbContext context) : base(factory, context)
        {
        }
    }
}