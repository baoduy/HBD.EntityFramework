using Microsoft.VisualStudio.TestTools.UnitTesting;
using HBD.Framework;
using HBD.EntityFramework.Base;

namespace HBD.EntityFramework.Test.Core
{
    [TestClass]
    public class SecurityStringTestCase
    {
        [TestMethod]
        [TestCategory("Fw.Entity.Security")]
        public void Can_Explicit_To_String()
        {
            CryptionString enString = "Duy";
            Assert.IsTrue(enString.DbValue.IsEncrypted());

            string en = enString;
            Assert.IsFalse(en.IsEncrypted());
        }

        [TestMethod]
        [TestCategory("Fw.Entity.Security")]
        public void Can_Cast_To_String()
        {
            object enString = new CryptionString { Value = "Duy" };
            Assert.IsTrue(((CryptionString)enString).DbValue.IsEncrypted());
            
            //Cannot cast directly from object string en = (string)enString;
            string en = (string)(CryptionString)enString;
            Assert.IsFalse(en.IsEncrypted());
        }
    }
}
