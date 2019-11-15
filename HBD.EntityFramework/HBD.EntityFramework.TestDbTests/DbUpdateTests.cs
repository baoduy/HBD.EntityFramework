using System.Data.Entity;
using System.Linq;
using FluentAssertions;
using HBD.EntityFramework.Core;
using HBD.EntityFramework.TestDal;
using HBD.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBD.EntityFramework.TestDbTests
{
    [TestClass]
    public class DbUpdateTests
    {
        [TestInitialize]
        public void Setup()
        {
            using (var f = SampleBootStrapper.GetExportOrDefault<IDbFactory>())
            {
                f.EnsureDbCreated();

                for (var i = 0; i < 10; i++)
                    f.For<Child>().Add(new Child { Name = $"Child {i}" });

                f.Save("Test");
            }
        }

        [TestMethod]
        public void UpdateChildren()
        {
            using (var f = SampleBootStrapper.GetExportOrDefault<IDbFactory>())
            {
                var p = new Parent { Name = "Parent 1" };
                p.ParentChildrens.AddRange(new[]
                {
                    new ParentChildren{ChildrenId = 1},
                    new ParentChildren{ChildrenId = 2},
                });

                f.For<Parent>().Add(p);
                f.Save("Test");

                var p1 = f.For<Parent>().AsQueryable(i => i.Id == p.Id).Include(i => i.ParentChildrens).AsNoTracking().First();

                p1.ParentChildrens.Count.Should().Be(2);
                p1.ParentChildrens.Add(new ParentChildren {ParentId = p1.Id, ChildrenId = 3 });

                f.For<Parent>().Update(p1).Includes(i => i.ParentChildrens);

                f.Save("Test");
            }
        }
    }
}
