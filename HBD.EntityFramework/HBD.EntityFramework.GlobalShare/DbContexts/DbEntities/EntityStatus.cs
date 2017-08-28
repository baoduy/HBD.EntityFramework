#if NETSTANDARD2_0 || NETSTANDARD1_6
using HBD.EntityFramework.Core;
using HBD.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
#else
using System.Data.Entity;
#endif

namespace HBD.EntityFramework.DbContexts.DbEntities
{
    public class EntityStatus<TEntity> where TEntity : class
    {
        public EntityStatus(TEntity entity, EntityState state, object byUser)
        {
            Entity = entity;
            State = state;
            ByUser = byUser;
        }

        public object ByUser { get; }
        public TEntity Entity { get; }
        public EntityState State { get; }
    }

    public class EntityStatus : EntityStatus<object>
    {
        public EntityStatus(object entity, EntityState state, object byUser)
            : base(entity, state, byUser)
        {
        }

        public EntityStatus<TEntity> Cast<TEntity>() where TEntity : class
            => new EntityStatus<TEntity>((TEntity)Entity, State, ByUser);
    }
}
