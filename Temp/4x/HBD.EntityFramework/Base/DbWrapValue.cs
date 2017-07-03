using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HBD.EntityFramework.Base
{
    [Serializable]
    public abstract class DbWrapValue<TValueType, TDbValueType> : IDbWrapValue<TValueType, TDbValueType>
    {
        public abstract TDbValueType DbValue { get; set; }

        [NotMapped]
        public TValueType Value { get; set; }
    }
}