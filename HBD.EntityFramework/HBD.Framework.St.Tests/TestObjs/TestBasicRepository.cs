using HBD.EntityFramework.DbContexts.BasicRepositories;
using HBD.EntityFramework.DbContexts.DbEntities;
using HBD.EntityFramework.DbContexts.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HBD.Framework.St.Tests.TestObjs
{
    public class TestBasicRepository<TEntity> : DbBasicRepository<TEntity> where TEntity : class, IDbEntity
    {
        public TestBasicRepository(IDbBasicRepositoryFactory factory, DbContext context) : base(factory, context)
        {
        }
    }
}