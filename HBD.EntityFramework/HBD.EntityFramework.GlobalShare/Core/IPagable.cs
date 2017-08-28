#region using

using System.Collections.Generic;

#endregion using

namespace HBD.EntityFramework.Core
{
    public interface IPagable
    {
        int PageIndex { get; }
        int PageSize { get; }
        int TotalItems { get; }
        int TotalPage { get; }
    }

    public interface IPagable<out TEntity> : IPagable
    {
        IReadOnlyCollection<TEntity> Items { get; }
    }
}