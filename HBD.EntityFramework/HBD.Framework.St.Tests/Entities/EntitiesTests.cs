using HBD.Framework.St.Tests.TestObjs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBD.Framework.St.Tests.Core
{
    [TestClass]
    public class EntitiesTests
    {
        [TestMethod]
        public void Tow_Object_Have_TheSameValue_ShouldBeEquals()
        {
            var v1 = new Ent1();
            var v2 = new Ent1();

            Assert.AreEqual(v1, v2);
        }

        [TestMethod]
        public void Tow_Object_Have_DifferentValues_ShouldBeDifference()
        {
            var v1 = new Ent1(1) { Name = "A" };
            var v2 = new Ent1(2) { Name = "A" };

            Assert.AreNotEqual(v1, v2);
        }

        [TestMethod]
        public void Tow_DifferentObject_Have_TheSameValue_ShouldBeDifference()
        {
            var v1 = new Ent1(1);
            var v2 = new Ent2("1");

            Assert.AreNotEqual(v1, v2);
        }

        [TestMethod]
        public void Tow_DifferentObject_Have_DifferentValues_ShouldBeDifference()
        {
            var v1 = new Ent1(1);
            var v2 = new Ent2("2");

            Assert.AreNotEqual(v1, v2);
        }
    }
}