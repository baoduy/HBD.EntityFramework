using System.ComponentModel.DataAnnotations.Schema;

namespace HBD.EntityFramework.Base
{
    public interface IDbWrapValue<TValueType, TDbValueType>
    {
        [NotMapped]
        TValueType Value { get; set; }

        TDbValueType DbValue { get; set; }
    }
}