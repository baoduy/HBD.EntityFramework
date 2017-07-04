using HBD.Framework.St.Tests.TestObjs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBD.Framework.St.Tests.Core
{
    [TestClass]
    public class ValuesTests
    {
        [TestMethod]
        public void Tow_ValueObject_Have_TheSameValue_ShouldBeEquals()
        {
            var v1 = new Val(1, "A");
            var v2 = new Val(1, "A");

            Assert.AreEqual(v1, v2);
            Assert.IsTrue(v1 == v2);
            Assert.IsFalse(v1 != v2);
        }

        [TestMethod]
        public void Tow_ValueObject_Have_DifferentValues_ShouldBeDifference()
        {
            var v1 = new Val(1, "A");
            var v2 = new Val(1, "B");

            Assert.AreNotEqual(v1, v2);
            Assert.IsFalse(v1 == v2);
            Assert.IsTrue(v1 != v2);
        }

        [TestMethod]
        public void Tow_DifferentValueObject_Have_TheSameValue_ShouldBeDifference()
        {
            var v1 = new Val(1, "A");
            var v2 = new Info(1, "A");

            Assert.AreNotEqual(v1, v2);
            Assert.IsFalse(v1 == v2);
            Assert.IsTrue(v1 != v2);
        }

        [TestMethod]
        public void Tow_DifferentValueObject_Have_DifferentValues_ShouldBeDifference()
        {
            var v1 = new Val(1, "A");
            var v2 = new Info(1, "B");

            Assert.AreNotEqual(v1, v2);
            Assert.IsFalse(v1 == v2);
            Assert.IsTrue(v1 != v2);
        }
    }
}