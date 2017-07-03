using EntityFramework.MappingAPI;
using HBD.Framework;
using System.Collections.Generic;
using System.Linq;

namespace System.Data.Entity.Utilities
{
    public static class EntityExtensions
    {
        public static IEnumerable<object> GetValues(this object entity, IPropertyMap[] properties)
            => properties.Select(p => p.PropertyName).Select(entity.GetValueFromProperty);
    }
}