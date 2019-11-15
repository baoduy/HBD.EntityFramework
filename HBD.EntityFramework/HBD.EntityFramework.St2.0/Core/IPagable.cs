#region using

using System.Collections;
using System.Collections.Generic;

#endregion using

namespace HBD.EntityFramework.Core
{
    public interface IPagable : IEnumerable
    {
        int PageIndex { get; }
        int PageSize { get; }
        int TotalItems { get; }
        int TotalPage { get; }
    }

    public interface IPagable<out TEntity> : IPagable, IEnumerable<TEntity>
    {
        IReadOnlyCollection<TEntity> Items { get; }
    }
}