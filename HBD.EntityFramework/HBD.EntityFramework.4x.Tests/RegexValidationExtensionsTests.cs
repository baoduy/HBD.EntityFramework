using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBD.EntityFramework._4x.Tests
{
    [TestClass]
    public class RegexValidationExtensionsTests
    {
        [TestMethod]
        public void IsEmail_Test()
        {
            Assert.IsTrue("baoduy@abc.com".IsEmail());
            Assert.IsFalse("baoduy@.com".IsEmail());
            Assert.IsFalse("baoduy.com".IsEmail());
            Assert.IsFalse("baoduy".IsEmail());
            Assert.IsFalse("@a.com".IsEmail());
        }
    }
}