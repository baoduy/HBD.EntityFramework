using HBD.EntityFramework.DbContexts.DbEntities;

namespace HBD.EntityFramework.Core
{
    public interface IPostSaveEventRegister
    {
    }

    /// <summary>
    ///     The data convention adapter for DbRepositoryFactories.
    ///     All adapters should be exported to Mef so that the DbRepositoryFactories will call the adapters when Saving the
    ///     entities to Database.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IPostSaveEventRegister<TEntity> : IPostSaveEventRegister where TEntity : class
    {
        void RaiseEvent(IDbFactory factory, EntityStatus<TEntity> entity);
    }
}