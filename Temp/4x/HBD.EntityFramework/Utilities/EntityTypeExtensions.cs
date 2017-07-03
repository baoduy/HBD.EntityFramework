using HBD.Framework.Core;

namespace System.Data.Entity.Utilities
{
    public static class EntityTypeExtensions
    {
        private const string ProxyNamespace = "System.Data.Entity.DynamicProxies";

        public static Type GetEntityType(this Type entityType)
        {
            Guard.ArgumentIsNotNull(entityType, "EntityType");

            if (entityType.Namespace == ProxyNamespace)
                return GetEntityType(entityType.BaseType);

            return entityType;
        }

        public static Type GetEntityType(this object entity)
        {
            Guard.ArgumentIsNotNull(entity, "Entity");

            Type entityType = null;

            if (entity is Type)
                entityType = (Type)entity;
            else entityType = entity.GetType();

            return entityType.GetEntityType();
        }
    }
}