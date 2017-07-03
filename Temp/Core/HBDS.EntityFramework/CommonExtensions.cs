#region using

using System.Linq;
using System.Threading.Tasks;
using HBDS.EntityFramework.Base;
using HBDS.Framework.Core;
using Microsoft.EntityFrameworkCore;

#endregion

namespace HBDS.EntityFramework
{
    public static class CommonExtensions
    {
        public static IPagable<TEntity> ToPagable<TEntity>(this IOrderedQueryable<TEntity> query, int pageIndex,
            int pageSize)
        {
            pageSize.ShouldGreaterThan(0, nameof(pageSize));
            pageIndex.ShouldGreaterThan(-1, nameof(pageIndex));

            var totalItems = query.Count();

            var itemIndex = pageIndex * pageSize;
            if (itemIndex < 0) itemIndex = 0; //Get first Page
            if (itemIndex >= totalItems) itemIndex = totalItems - pageSize; //Get last page.

            var items = pageSize >= totalItems ? query : query.Skip(itemIndex).Take(pageSize);

            return new Pagable<TEntity>(pageIndex, pageSize, totalItems, items.ToList());
        }

        public static async Task<IPagable<TEntity>> ToPagableAsync<TEntity>(this IOrderedQueryable<TEntity> query,
            int pageIndex, int pageSize)
        {
            pageSize.ShouldGreaterThan(0, nameof(pageSize));
            pageIndex.ShouldGreaterThan(-1, nameof(pageIndex));

            var totalItems = await query.CountAsync();

            var itemIndex = pageIndex * pageSize;
            if (itemIndex < 0) itemIndex = 0; //Get first Page
            if (itemIndex >= totalItems) itemIndex = totalItems - pageSize; //Get last page.

            var items = pageSize >= totalItems ? query : query.Skip(itemIndex).Take(pageSize);

            return new Pagable<TEntity>(pageIndex, pageSize, totalItems, await items.ToListAsync());
        }
    }
}