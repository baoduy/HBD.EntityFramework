using HBD.Framework.St.Tests.TestObjs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBD.Framework.St.Tests.Repositories
{
    [TestClass]
    public class FactoryTests
    {
        [TestMethod]
        public void Test_The_Repository_Alway_Singleton()
        {
            var factory = new TestFactory(new TestDbContext(), true);
            var p1 = factory.For<Person>();
            var p2 = factory.For<Person>();

            Assert.AreEqual(p1, p2);
        }
    }
}