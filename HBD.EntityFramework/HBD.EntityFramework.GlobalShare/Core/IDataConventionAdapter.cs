using HBD.EntityFramework.DbContexts.DbEntities;

namespace HBD.EntityFramework.Core
{
    public interface IDataConventionAdapter
    {
    }

    /// <summary>
    ///     The data convention adapter for DbRepositoryFactories.
    ///     All adapters should be exported to Mef so that the DbRepositoryFactories will call tthe adapters when Saving the
    ///     entities to Database.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IDataConventionAdapter<TEntity> : IDataConventionAdapter where TEntity : class
    {
        void ApplyFor(EntityStatus<TEntity> entity);
    }
}