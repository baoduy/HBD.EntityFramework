using AutoMapper;
using HBD.EntityFramework.DbContexts.DbEntities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HBD.EntityFramework.Sample
{
    internal static class Extensions
    {
        public static T To<T>(this object @this)
            => Mapper.Map<T>(@this);

        public static IEnumerable<T> To<T>(this IEnumerable @this)
            => Mapper.Map<IEnumerable<T>>(@this);

        public static IPagable<TTo> ToPage<TFrom, TTo>(this IPagable<TFrom> @this)
            => new Pagable<TTo>(@this.PageIndex, @this.PageSize, @this.TotalPage, @this.Items.To<TTo>().ToList());

        public static void AddRange<T>(this HashSet<T> @this, IEnumerable<T> collection)
        {
            foreach (var item in collection)
                @this.Add(item);
        }
    }
}