using HBD.Framework;
using System;
using System.Linq;

namespace HBD.EntityFramework.GlobalShare.Core
{
    /// <summary>
    /// The Value is representative of the ValueObjec in domain-driven pattern.
    /// This object should be designed as Immutable object.
    /// </summary>
    public abstract class Value<TValue> : IValueObject, IEquatable<TValue> where TValue : IValueObject
    {
        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            if (obj is TValue b)
                return Equals(b);

            return false;
        }

        public override int GetHashCode() => this.GetType().FullName.GetHashCode() * 397 ^ this.GetValuesHashCode();

        protected virtual int GetValuesHashCode()
        {
            int result = 0;

            foreach (var item in this.GetType().GetProperties().Where(a => a.CanRead))
            {
                var v = item.GetValue(this);

                if (v is int i)
                    result = result * 397 ^ i;
                else result = result * 397 ^ v.GetHashCode();
            }

            return result;
        }

        public abstract bool Equals(TValue other);

        public static bool operator ==(Value<TValue> objA, IValueObject objB)
        {
            if (ReferenceEquals(objA, null) && ReferenceEquals(objB, null)) return true;
            if (ReferenceEquals(objA, null) || ReferenceEquals(objB, null)) return false;
            return objA.Equals(objB);
        }

        public static bool operator !=(Value<TValue> objA, IValueObject objB) => !(objA == objB);
    }
}