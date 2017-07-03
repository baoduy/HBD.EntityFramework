#region using

using System.Collections.Generic;

#endregion

namespace HBDS.EntityFramework.Base
{
    public class Pagable<TEntity> : IPagable<TEntity>
    {
        internal Pagable(int pageIndex, int pageSize, int totalItems, List<TEntity> items)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalItems = totalItems;
            Items = items;
        }

        public int PageIndex { get; }
        public int PageSize { get; }
        public int TotalItems { get; }
        public int TotalPage => TotalItems / PageSize + (TotalItems % PageSize > 0 ? 1 : 0);
        public List<TEntity> Items { get; }
    }
}