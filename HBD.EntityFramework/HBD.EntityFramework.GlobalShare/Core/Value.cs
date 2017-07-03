using System;

namespace HBD.EntityFramework.GlobalShare.Core
{
    /// <summary>
    /// The Value is representive of the ValueObjec in domain-driven pattern.
    /// This object should be designed as Immtable object.
    /// </summary>
    public abstract class Value<TValue> : IValueObject, IEquatable<TValue> where TValue : IValueObject
    {
        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            if (obj is TValue)
                return Equals((TValue)obj);
            return false;
        }

        public override int GetHashCode() => this.GetType().FullName.GetHashCode() ^ this.GetValuesHashCode();

        protected virtual int GetValuesHashCode()
        {
            return 0;
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
