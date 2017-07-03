using System;

namespace HBD.EntityFramework.Base
{
    public class DbStringnable<T> : DbWrapValue<T, string>
    {
        public override string DbValue
        {
            get { return this.Value?.ToString() ?? string.Empty; }
            set { this.Value = (T)Convert.ChangeType(value, typeof(T)); }
        }

        public override string ToString() => this.DbValue;

        public static implicit operator string(DbStringnable<T> value) => value.DbValue;

        public static implicit operator DbStringnable<T>(string value)
        {
            return new DbStringnable<T> { DbValue = value };
        }
    }
}