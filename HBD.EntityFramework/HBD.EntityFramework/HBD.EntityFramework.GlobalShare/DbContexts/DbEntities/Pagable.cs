#region using

using System.Collections;
using HBD.EntityFramework.Core;
using System.Collections.Generic;
using System.Collections.ObjectModel;

#endregion using

namespace HBD.EntityFramework.DbContexts.DbEntities
{
    public class Pagable<TEntity> : IPagable<TEntity>
    {
        public Pagable(int pageIndex, int pageSize, int totalItems, IList<TEntity> items)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalItems = totalItems;
            Items = new ReadOnlyCollection<TEntity>(items);
        }

        public int PageIndex { get; }
        public int PageSize { get; }
        public int TotalItems { get; }
        public int TotalPage => TotalItems / PageSize + (TotalItems % PageSize > 0 ? 1 : 0);

        public IReadOnlyCollection<TEntity> Items { get; }
        public IEnumerator<TEntity> GetEnumerator() => Items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}