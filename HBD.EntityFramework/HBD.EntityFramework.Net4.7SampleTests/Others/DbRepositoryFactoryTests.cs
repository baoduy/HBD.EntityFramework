using HBD.EntityFramework.TestSample;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace HBD.EntityFramework.SampleTests.Others
{
    [TestClass]
    public class DbRepositoryFactoryTests
    {
        [TestMethod]
        public void Can_Get_Containter()
        {
            var a = SampleBootStrapper.GetExportOrDefault<CompositionContainer>()
                ?? SampleBootStrapper.GetExportOrDefault<ICompositionService>();

            Assert.IsNotNull(a);
        }
    }
}
