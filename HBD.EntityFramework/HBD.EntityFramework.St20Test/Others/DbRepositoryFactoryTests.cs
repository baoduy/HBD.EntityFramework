using FluentAssertions;
using HBD.EntityFramework.Core;
using HBD.EntityFramework.TestSample;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HBD.Framework;



#if NETSTANDARD2_0 || NETSTANDARD1_6 || NETCOREAPP1_1 || NETCOREAPP2_0
using System.Composition;
#else
using System.ComponentModel.Composition.Hosting;
#endif

namespace HBD.EntityFramework.SampleTests.Others
{
    [TestClass]
    public class DbRepositoryFactoryTests
    {
        [TestMethod]
        public void Can_Get_Containter()
        {
            dynamic a;

#if NETSTANDARD2_0 || NETSTANDARD1_6 || NETCOREAPP1_1 || NETCOREAPP2_0
            a = SampleBootStrapper.Default.Container.GetExport<CompositionContext>();
#else
            a = SampleBootStrapper.Default.Container.GetExportedValue<CompositionContainer>();
#endif
            Assert.IsNotNull(a);
        }

        [TestMethod]
        public void Container_Should_Not_Be_Null()
        {
            IDbRepositoryFactory f;

#if NETSTANDARD2_0 || NETSTANDARD1_6 || NETCOREAPP1_1 || NETCOREAPP2_0
            f = SampleBootStrapper.Default.Container.GetExport<IDbRepositoryFactory>();
#else
            f = SampleBootStrapper.Default.Container.GetExportedValue<IDbRepositoryFactory>();
#endif
            f.Should().NotBeNull("The DbRepositoryFactory");
        }
    }
}
