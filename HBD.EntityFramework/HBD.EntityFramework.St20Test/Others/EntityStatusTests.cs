using FluentAssertions;
using HBD.EntityFramework.DbContexts.DbEntities;
using HBD.EntityFramework.Sample.DbEntities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HBD.EntityFramework.Core;

#if NETSTANDARD2_0 || NETCOREAPP1_1 || NETCOREAPP2_0
using Microsoft.EntityFrameworkCore;
#else
using System.Data.Entity;
#endif

namespace HBD.EntityFramework.SampleTests.Others
{
    [TestClass]
    public class EntityStatusTests
    {
        [TestMethod]
        public void Able_To_Cast_To_Generic()
        {
            var e = new EntityStatus(new PersonDb(), EntityState.Added,new object());
            var t = e.Cast<PersonDb>();

            t.Entity.Should().Be(e.Entity);
            t.State.Should().Be(e.State);
            t.ByUser.Should().Be(e.ByUser);
        }

        [TestMethod]
        public void PersonDb_Should_Be_IDbEntity_Int_String()
        {
            (new PersonDb() is IDbEntity<int, string>)
                 .Should().BeTrue();
        }
    }
}
