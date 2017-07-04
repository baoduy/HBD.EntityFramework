using HBD.Framework.St.Tests.TestObjs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using System.Data.Entity;

namespace HBD.Framework.St.Tests.Repositories
{
    [TestClass]
    public class EntityRepositoryTests
    {
        public const string TestDbName = "Test_Database";

        [TestMethod]
        public void Test_AutoDispose_AutoDisposeDbContext_DbContext()
        {
            var m = new Mock<DbContext>();
            m.Protected().Setup("Dispose", true).Verifiable();

            var factory = new TestFactory(m.Object, true);
            factory.Dispose();

            m.Protected().Verify("Dispose", Times.Once(), true);
        }

        [TestMethod]
        public void Test_AutoDispose_NotAutoDisposeDbContext_DbContext()
        {
            var m = new Mock<DbContext>();
            m.Protected().Setup("Dispose", true).Verifiable();

            var factory = new TestFactory(m.Object, false);
            factory.Dispose();

            m.Protected().Verify("Dispose", Times.Never(), true);
        }
    }
}