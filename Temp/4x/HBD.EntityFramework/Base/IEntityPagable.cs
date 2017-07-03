using System.Collections.Generic;

namespace HBD.EntityFramework.Base
{
    public interface IEntityPagable<TEntity> : IPagable, IList<TEntity>, IEnumerable<TEntity> where TEntity : class { }
}