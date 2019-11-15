using HBD.EntityFramework.Core;

namespace HBD.EntityFramework.TestDal
{
    public interface ITestEntity : IDbEntity<int, string>
    {
    }
}
