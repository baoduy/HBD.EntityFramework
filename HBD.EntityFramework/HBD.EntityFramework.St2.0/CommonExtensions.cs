﻿#region using

using System.Linq;
using System.Threading.Tasks;
using HBD.Framework.Core;
using HBD.EntityFramework.DbContexts.DbEntities;
using HBD.EntityFramework.Core;
using Z.EntityFramework.Plus;

#if NETSTANDARD2_0

using Microsoft.EntityFrameworkCore;

#else
using System.Data.Entity;
#endif

#endregion using

namespace HBD.EntityFramework
{
    public static class CommonExtensions
    {
        public static IPagable<TEntity> ToPagable<TEntity>(this IOrderedQueryable<TEntity> query, int pageIndex,
            int pageSize)
        {
            pageSize.ShouldGreaterThan(0, nameof(pageSize));
            pageIndex.ShouldGreaterThan(-1, nameof(pageIndex));

            //Catch to improve the performance
            var totalItems = query.DeferredCount().FromCache();

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

            //Catch to improve the performance
            var totalItems = await query.DeferredCount().FromCacheAsync();

            var itemIndex = pageIndex * pageSize;
            if (itemIndex < 0) itemIndex = 0; //Get first Page
            if (itemIndex >= totalItems) itemIndex = totalItems - pageSize; //Get last page.

            var items = pageSize >= totalItems ? query : query.Skip(itemIndex).Take(pageSize);

            return new Pagable<TEntity>(pageIndex, pageSize, totalItems, await items.ToListAsync());
        }
    }
}