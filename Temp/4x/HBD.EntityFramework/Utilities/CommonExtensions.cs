using HBD.Framework;
using HBD.Framework.Core;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Core;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Linq.Expressions;

namespace System.Data.Entity.Utilities
{
    public static class CommonExtensions
    {
        //public static TVal GetFromCache<TKey, TVal>(this ConcurrentDictionary<TKey, TVal> dictionary, TKey key, Func<TKey, TVal> valueFactory)
        //{
        //    lock (dictionary)
        //    {
        //        return dictionary.GetOrAdd(key, valueFactory);
        //    }
        //}

        public static StringPropertyConfiguration HasMaxLengthInAttribute<TStructuralType>
            (this StringPropertyConfiguration @this, Expression<Func<TStructuralType, object>> propertyExpression) where TStructuralType : class
        {
            Guard.ArgumentIsNotNull(propertyExpression, "PropertyExpression");

            var property = propertyExpression.GetProperties().FirstOrDefault();
            var att = property.GetAttribute<StringLengthAttribute>();
            if (att == null) throw new ObjectNotFoundException($"Attribute '{typeof(TStructuralType).Name}' is not found on property '{property.Name}'");

            @this.HasMaxLength(att.MaximumLength);
            return @this;
        }
    }
}