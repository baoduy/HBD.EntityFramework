#region using

using System.Collections.Generic;

#endregion using

namespace HBD.EntityFramework.DbContexts.DbEntities
{
    public interface IPagable
    {
        int PageIndex { get; }
        int PageSize { get; }
        int TotalItems { get; }
        int TotalPage { get; }
    }

    public interface IPagable<TEntity> : IPagable
    {
        IReadOnlyCollection<TEntity> Items { get; }
    }
}