#if !NETSTANDARD2_0
using HBD.EntityFramework.DbContexts.DbEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;

namespace HBD.EntityFramework
{
    public static class EntityMappingExtensions
    {
        public static DbModelBuilder RegisterMapping<TEntity, TMapping>(this DbModelBuilder builder)
           where TMapping : IEntityMappingConfiguration<TEntity>
           where TEntity : class
        {
            var mapper = (IEntityMappingConfiguration<TEntity>)Activator.CreateInstance(typeof(TMapping));
            mapper.Map(builder.Entity<TEntity>());
            return builder;
        }

        private static IEnumerable<Type> GetMappingTypes(this Assembly assembly, Type mappingInterface)
            => assembly.GetTypes().Where(x => !x.GetTypeInfo().IsAbstract && x.GetTypeInfo().GetInterfaces().Any(y => y.GetTypeInfo().IsGenericType && y.GetGenericTypeDefinition() == mappingInterface));

        public static void RegisterMappingFromAssembly(this DbModelBuilder modelBuilder, Assembly assembly)
        {
            var method = typeof(EntityMappingExtensions).GetTypeInfo().GetMethod(nameof(RegisterMapping));
            var mappingTypes = assembly.GetMappingTypes(typeof(IEntityMappingConfiguration<>));

            foreach (var type in mappingTypes)
            {
                var eType = type.GetTypeInfo().GetInterfaces().First(a => a.GetTypeInfo().IsGenericType).GetTypeInfo().GetGenericArguments().First();
                var md = method.MakeGenericMethod(new[] { eType, type });
                md.Invoke(null, new[] { modelBuilder });
            }
        }
    }
}
#endif