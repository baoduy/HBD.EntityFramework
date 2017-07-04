using HBD.Framework.St.Tests.TestObjs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBD.Framework.St.Tests.BasicRepositories
{
    [TestClass]
    public class BasicFactoryTests
    {
        [TestMethod]
        public void Test_Basic_The_Repository_Alway_Singleton()
        {
            var factory = new TestBasicFactory(new TestDbContext(), true);
            var p1 = factory.For<Person>();
            var p2 = factory.For<Person>();

            Assert.AreEqual(p1, p2);
        }
    }
}