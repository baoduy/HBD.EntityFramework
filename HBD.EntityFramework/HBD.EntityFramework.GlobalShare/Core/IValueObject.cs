using HBD.EntityFramework.Core;

namespace HBD.EntityFramework.GlobalShare.Core
{
    /// <summary>
    /// The IValueObject is representative of the ValueObjec in domain-driven pattern.
    /// This object should be designed as Immutable object.
    /// The properties should be read only.
    /// </summary>
    public interface IValueObject : IDto
    {
    }
}