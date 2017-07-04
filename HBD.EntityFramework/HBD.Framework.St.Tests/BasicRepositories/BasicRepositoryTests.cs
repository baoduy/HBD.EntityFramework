using HBD.Framework.St.Tests.TestObjs;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using System.Threading.Tasks;

namespace HBD.Framework.St.Tests.BasicRepositories
{
    [TestClass]
    public class BasicRepositoryTests
    {
        public const string TestDbName = "Test_Basic_Database";

        [TestMethod]
        public void Test_Basic_AutoDispose_AutoDispose_DbContext()
        {
            var m = new Mock<DbContext>();
            m.Setup(a => a.Dispose()).Verifiable();

            var factory = new TestBasicFactory(m.Object, true);
            factory.Dispose();

            m.Verify(a => a.Dispose(), Times.Once());
        }

        [TestMethod]
        public void Test_Basic_AutoDispose_NotAutoDispose_DbContext()
        {
            var m = new Mock<DbContext>();
            m.Setup(a => a.Dispose()).Verifiable();

            var factory = new TestBasicFactory(m.Object, false);
            factory.Dispose();

            m.Verify(a => a.Dispose(), Times.Never());
        }

        [TestMethod]
        public void Test_Basic_Create_Person()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(TestDbName)
                .Options;

            var db = new TestDbContext(options);
            db.Database.EnsureCreated();

            using (var factory = new TestBasicFactory(new TestDbContext(options), true))
            {
                var p = factory.For<Person>();
                p.Add(new Person { FirstName = "Duy" });
                Assert.IsTrue(factory.Save() == 1);

                var p1 = p.AsQueryable().AsNoTracking().First();
                Assert.IsTrue(p1.FirstName == "Duy");
                Assert.IsTrue(p1.Id > 0);

                p1.LastName = "Hoang";
                p.Update(p1);
                factory.Save();

                var p2 = p.AsQueryable().AsNoTracking().First();
                Assert.IsTrue(p2.LastName == "Hoang");
            }
        }

        [TestMethod]
        public void Test_Basic_Select_NoPerson()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(databaseName: "New_Database")
                .Options;

            using (var factory = new TestBasicFactory(new TestDbContext(options), true))
            {
                var p = factory.For<Person>();
                Assert.IsFalse(p.AsQueryable().Any());
            }
        }

        [TestMethod]
        public async Task Test_Basic_Update_Person()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(databaseName: TestDbName)
                .Options;

            using (var factory = new TestBasicFactory(new TestDbContext(options), true))
            {
                var p = factory.For<Person>();
                var n = new Person { FirstName = "Duy" };
                p.Add(n);
                await factory.SaveAsync();

                var a = p.AsQueryable().First(t => t.Id == n.Id);
                Assert.IsNull(a.LastName);

                a.LastName = "Hoang";
                p.Update(a);
                await factory.SaveAsync(true);

                var b = p.AsQueryable().First(t => t.Id == n.Id);
                Assert.IsTrue(b.LastName == "Hoang");
            }
        }

        [TestMethod]
        public async Task Test_Basic_Update_Deattached_Person()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(databaseName: TestDbName)
                .Options;

            using (var factory = new TestBasicFactory(new TestDbContext(options), true))
            {
                var p = factory.For<Person>();

                p.DeleteAll();
                await factory.SaveAsync();

                p.Add(new Person { FirstName = "Duy" });
                await factory.SaveAsync();

                var a = p.AsQueryable().AsNoTracking().FirstOrDefault();
                Assert.IsNotNull(a);
                Assert.IsNull(a.LastName);

                a.LastName = "Hoang";
                p.Update(a);
                await factory.SaveAsync(true);

                var b = p.AsQueryable().FirstOrDefault(t => t.Id == a.Id);
                Assert.IsNotNull(b);
                Assert.IsTrue(b.LastName == "Hoang");
            }
        }

        [TestMethod]
        public async Task Test_Basic_Delete_Person()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(databaseName: TestDbName)
                .Options;

            using (var factory = new TestBasicFactory(new TestDbContext(options), true))
            {
                var p = factory.For<Person>();
                p.DeleteAll();
                await factory.SaveAsync();

                Assert.IsTrue(p.AsQueryable().Count() == 0);

                p.Add(new Person { FirstName = "Duy" });
                await factory.SaveAsync();

                var a = p.AsQueryable().FirstOrDefault();
                Assert.IsNotNull(a);
                Assert.IsNull(a.LastName);

                p.Delete(a);
                await factory.SaveAsync(true);

                var b = p.AsQueryable().FirstOrDefault(t => t.Id == a.Id);
                Assert.IsNull(b);
            }
        }

        /// <summary>
        /// This testing only works with SQL Server because RowVersion is not supporting by InMemoryDatabase.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(DbUpdateConcurrencyException))]
        public void Concurrency_UpdateCheck()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseSqlServer("Data Source=WIN-HFIQC0VT0PC\\SQLEXPRESS;Initial Catalog=Test;Integrated Security=True")
                //.UseInMemoryDatabase(TestDbName)
                .Options;

            var factory1 = new TestBasicFactory(new TestDbContext(options), true);
            var factory2 = new TestBasicFactory(new TestDbContext(options), true);

            var p1 = factory1.For<Person>();
            var p2 = factory2.For<Person>();

            //Clean Db
            p1.DeleteAll();
            factory1.Save();

            p1.Add(new Person { FirstName = "Duy" });
            factory1.Save();

            var ps1 = p1.AsQueryable().AsNoTracking().First();
            var ps2 = p2.AsQueryable().AsNoTracking().First();

            ps2.LastName = "Hoang";
            p2.Update(ps2);
            factory2.Save();

            ps1.LastName = "Bao";
            p1.Update(ps1);
            factory1.Save();

            Assert.IsNotNull(ps1.RowVersion);
            Assert.IsNotNull(ps2.RowVersion);
            Assert.AreNotEqual(ps1.RowVersion, ps2.RowVersion);
        }
    }
}