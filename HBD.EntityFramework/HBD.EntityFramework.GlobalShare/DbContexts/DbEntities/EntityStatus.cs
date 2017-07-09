#if NETSTANDARD2_0 || NETSTANDARD1_6
using Microsoft.EntityFrameworkCore;
#else
using System.Data.Entity;
#endif

namespace HBD.EntityFramework.DbContexts.DbEntities
{
    public class EntityStatus
    {
        public EntityStatus(object entity, EntityState state)
        {
            Entity = entity;
            State = state;
        }

        public object Entity { get; }
        public EntityState State { get; }
    }
}
