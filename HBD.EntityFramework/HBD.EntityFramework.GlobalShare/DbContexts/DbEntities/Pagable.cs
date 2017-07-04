#region using

using System.Collections.Generic;

#endregion using

namespace HBD.EntityFramework.DbContexts.DbEntities
{
    public class Pagable<TEntity> : IPagable<TEntity>
    {
        public Pagable(int pageIndex, int pageSize, int totalItems, List<TEntity> items)
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
        public IReadOnlyCollection<TEntity> Items { get; }
    }
}