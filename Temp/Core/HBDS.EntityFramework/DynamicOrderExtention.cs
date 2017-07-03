#region using

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using HBDS.Framework.Core;

#endregion

namespace HBDS.EntityFramework
{
    public static class DynamicOrderExtention
    {
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string[] properties)
            =>
                properties.Aggregate<string, IOrderedQueryable<T>>(null,
                    (current, p) => current == null ? source.OrderBy(p) : current.ThenBy(p));

        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string[] properties)
            =>
                properties.Aggregate<string, IOrderedQueryable<T>>(null,
                    (current, p) => current == null ? source.OrderByDescending(p) : current.ThenByDescending(p));

        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string property)
            => ApplyOrder(source, property, "OrderBy");

        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string property)
            => ApplyOrder(source, property, "OrderByDescending");

        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string property)
            => ApplyOrder(source, property, "ThenBy");

        public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> source, string property)
            => ApplyOrder(source, property, "ThenByDescending");

        private static IOrderedQueryable<T> ApplyOrder<T>(IQueryable<T> source, string property, string methodName)
        {
            Guard.ArgumentIsNotNull(property, nameof(property));
            Guard.ArgumentIsNotNull(methodName, nameof(methodName));

            var props = property.Split('.');
            var type = typeof(T);
            var arg = Expression.Parameter(type, "x");
            Expression expr = arg;

            //Get Property of Child Object and create expresstion OrderMethod(t=>t.Property)
            foreach (var prop in props)
            {
                // use reflection (not ComponentModel) to mirror LINQ
                var pi = type.GetRuntimeProperty(prop);
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }

            //Create generic order method
            var delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            var lambda = Expression.Lambda(delegateType, expr, arg);

            var result = typeof(Queryable).GetRuntimeMethods().First(
                    method => method.Name == methodName
                              && method.IsGenericMethodDefinition
                              && method.GetGenericArguments().Length == 2
                              && method.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), type)
                .Invoke(null, new object[] {source, lambda});

            return (IOrderedQueryable<T>) result;
        }
    }
}