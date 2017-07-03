using HBD.EntityFramework.Repositories;
using HBD.Framework.St.Tests.TestObjs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBD.Framework.St.Tests.Repositories
{
    [TestClass]
    public class EntityFactoryTests
    {
        [TestMethod]
        public void Test_The_Repository_Alway_Singleton()
        {
            var factory = new EntityRepositoryFactory(new TestDbContext(), true);
            var p1 = factory.RepositoryFor<Person>();
            var p2 = factory.RepositoryFor<Person>();

            Assert.AreEqual(p1, p2);
        }
    }
}
