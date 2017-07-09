using HBD.EntityFramework.DbContexts.DbEntities;
using HBD.EntityFramework.DbContexts.DbRepositories;
using HBD.EntityFramework.DbContexts.Interfaces;

#if NETSTANDARD2_0 || NETSTANDARD1_6
using Microsoft.EntityFrameworkCore;
#else
using System.Data.Entity;
#endif

namespace HBD.EntityFramework.Sample.DbContexts
{
    public class SampleRepository<TEntity> : DbRepo<TEntity> where TEntity :class, IDbEntity
    {
        public SampleRepository(IDbRepoFactory factory, DbContext context) : base(factory, context)
        {
        }
    }
}