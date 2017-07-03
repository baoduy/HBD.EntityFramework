#region using

using System;
using HBD.EntityFramework.Core;

#if NETSTANDARD2_0 || NETSTANDARD1_6
using Microsoft.EntityFrameworkCore;
#else
using System.Data.Entity;
#endif

#endregion

namespace HBD.EntityFramework.Repositories
{
    public class EntityRepository<TEntity> : Repository<TEntity> where TEntity : class, IEntity
    {
        public EntityRepository(IRepositoryFactory factory, DbContext context) : base(factory, context)
        {
        }
    }
}