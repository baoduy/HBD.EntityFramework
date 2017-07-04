using HBD.Framework.St.Tests.TestObjs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.DataAnnotations;

namespace HBD.EntityFramework._4x.Tests
{
    [TestClass]
    public class ValidationTests
    {
        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Validate_Email()
        {
            var p = new Person { FirstName = "Duy", Email = "abc" };
            Validator.ValidateObject(p);
        }
    }
}