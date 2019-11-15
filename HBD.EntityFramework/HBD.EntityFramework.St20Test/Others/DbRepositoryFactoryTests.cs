using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using HBD.EntityFramework.Core;
using HBD.EntityFramework.Sample.DbEntities;
using HBD.EntityFramework.St20Test.Others;
using HBD.EntityFramework.TestSample;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#if NETSTANDARD2_0 || NETCOREAPP1_1 || NETCOREAPP2_0
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

#if NETSTANDARD2_0 || NETCOREAPP1_1 || NETCOREAPP2_0
            a = SampleBootStrapper.Default.Container.GetExport<CompositionContext>();
#else
            a = SampleBootStrapper.Default.Container.GetExportedValue<CompositionContainer>();
#endif
            Assert.IsNotNull(a);
        }

        [TestMethod]
        public void Container_Should_Not_Be_Null()
        {
            IDbFactory f;

#if NETSTANDARD2_0 || NETCOREAPP1_1 || NETCOREAPP2_0
            f = SampleBootStrapper.Default.Container.GetExport<IDbFactory>();
#else
            f = SampleBootStrapper.Default.Container.GetExportedValue<IDbRepositoryFactory>();
#endif
            f.Should().NotBeNull("The DbRepositoryFactory");
        }

        [TestMethod]
        public async Task SaveANewPerson_AllRelatedEventsRaised_Async()
        {
            var pre = SampleBootStrapper.GetExportOrDefault<PersonPreSaveEvent>();
            var post = SampleBootStrapper.GetExportOrDefault<PersonPostSaveEvent>();

            pre.IsCalled = post.IsCalled = false;

            using (var fc = SampleBootStrapper.GetExportOrDefault<IDbFactory>())
            {
                fc.For<PersonDb>().Add(new PersonDb{FirstName = "Duy", LastName = "Hoang"});
                await fc.SaveAsync("Duy");
            }

            pre.IsCalled.Should().BeTrue();
            post.IsCalled.Should().BeTrue();
        }

        [TestMethod]
        public void SaveANewPerson_AllRelatedEventsRaised()
        {
            var pre = SampleBootStrapper.GetExportOrDefault<PersonPreSaveEvent>();
            var post = SampleBootStrapper.GetExportOrDefault<PersonPostSaveEvent>();

            pre.IsCalled = post.IsCalled = false;

            using (var fc = SampleBootStrapper.GetExportOrDefault<IDbFactory>())
            {
                fc.For<PersonDb>().Add(new PersonDb { FirstName = "Duy", LastName = "Hoang" });
                fc.Save("Duy");
            }

            pre.IsCalled.Should().BeTrue();
            post.IsCalled.Should().BeTrue();
        }

        [TestMethod]
        public void GetChangingEntities()
        {
            using (var fc = SampleBootStrapper.GetExportOrDefault<IDbFactory>())
            {
                fc.For<PersonDb>().Add(new PersonDb { FirstName = "Duy", LastName = "Hoang" });
                fc.GetChangingEntities<PersonDb>().Count().Should().Be(1);
            }
        }

        [TestMethod]
        public void Repo_GetChangingEntities()
        {
            using (var fc = SampleBootStrapper.GetExportOrDefault<IDbFactory>())
            {
                fc.For<PersonDb>().Add(new PersonDb { FirstName = "Duy", LastName = "Hoang" });
                fc.For<PersonDb>().GetChangingEntities().Count().Should().Be(1);
            }
        }
    }
}
