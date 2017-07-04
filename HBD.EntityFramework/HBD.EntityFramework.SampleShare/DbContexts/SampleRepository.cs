using HBD.EntityFramework.DbContexts.DbEntities;
using HBD.EntityFramework.DbContexts.DbRepositories;

#if NETSTANDARD2_0 || NETSTANDARD1_6
using Microsoft.EntityFrameworkCore;
#else
using System.Data.Entity;
#endif

namespace HBD.EntityFramework.Sample.DbContexts
{
    public class SampleRepository<TEntity> : DbRepository<TEntity> where TEntity :class, IDbEntity<int, string>
    {
        public SampleRepository(DbRepositoryFactory factory, DbContext context) : base(factory, context)
        {
        }
    }
}