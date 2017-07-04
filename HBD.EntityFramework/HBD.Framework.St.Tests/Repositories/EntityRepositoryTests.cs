using HBD.EntityFramework.Exceptions;
using HBD.Framework.St.Tests.TestObjs;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HBD.Framework.St.Tests.Repositories
{
    [TestClass]
    public class RepositoryTests
    {
        public const string TestDbName = "Test_Database";

        [TestMethod]
        public void Test_AutoDispose_AutoDispose_DbContext()
        {
            var m = new Mock<DbContext>();
            m.Setup(a => a.Dispose()).Verifiable();

            var factory = new TestFactory(m.Object, true);
            factory.Dispose();

            m.Verify(a => a.Dispose(), Times.Once());
        }

        [TestMethod]
        public void Test_AutoDispose_NotAutoDispose_DbContext()
        {
            var m = new Mock<DbContext>();
            m.Setup(a => a.Dispose()).Verifiable();

            var factory = new TestFactory(m.Object, false);
            factory.Dispose();

            m.Verify(a => a.Dispose(), Times.Never());
        }

        [TestMethod]
        public void Test_Create_Person()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(TestDbName)
                .Options;

            var db = new TestDbContext(options);
            db.Database.EnsureCreated();

            using (var factory = new TestFactory(new TestDbContext(options), true))
            {
                var p = factory.For<Person>();
                p.Add(new Person { FirstName = "Duy" }, "Duy");
                Assert.IsTrue(factory.Save() == 1);

                var p1 = p.AsQueryable().AsNoTracking().First();
                Assert.IsTrue(p1.FirstName == "Duy");
                Assert.IsTrue(p1.Id > 0);

                p1.LastName = "Hoang";
                p.Update(p1, "Hoang");
                factory.Save();

                var p2 = p.AsQueryable().AsNoTracking().First();
                Assert.IsTrue(p2.LastName == "Hoang");

                Assert.AreEqual(p2.CreatedBy, "Duy");
                Assert.IsTrue(p2.CreatedTime > DateTime.MinValue);
            }
        }

        [TestMethod]
        public void Test_Select_NoPerson()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(databaseName: "New_Database")
                .Options;

            using (var factory = new TestFactory(new TestDbContext(options), true))
            {
                var p = factory.For<Person>();
                Assert.IsFalse(p.AsQueryable().Any());
            }
        }

        [TestMethod]
        public async Task Test_Update_Person()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(databaseName: TestDbName)
                .Options;

            using (var factory = new TestFactory(new TestDbContext(options), true))
            {
                var p = factory.For<Person>();
                var n = new Person { FirstName = "Duy" };
                p.Add(n, "Duy");
                await factory.SaveAsync();

                var a = p.AsQueryable().First(t => t.Id == n.Id);
                Assert.IsNull(a.LastName);

                a.LastName = "Hoang";
                p.Update(a, "Duy");
                await factory.SaveAsync(true);

                var b = p.AsQueryable().First(t => t.Id == n.Id);
                Assert.IsTrue(b.LastName == "Hoang");

                Assert.AreEqual(b.UpdatedBy, "Duy");
                Assert.IsNotNull(b.UpdatedTime);
            }
        }

        [TestMethod]
        public async Task Test_Update_Deattached_Person()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(databaseName: TestDbName)
                .Options;

            using (var factory = new TestFactory(new TestDbContext(options), true))
            {
                var p = factory.For<Person>();

                p.DeleteAll("Duy");
                await factory.SaveAsync();

                p.Add(new Person { FirstName = "Duy" }, "Duy");
                await factory.SaveAsync();

                var a = p.AsQueryable().AsNoTracking().FirstOrDefault();
                Assert.IsNotNull(a);
                Assert.IsNull(a.LastName);

                a.LastName = "Hoang";
                p.Update(a, "Duy");
                await factory.SaveAsync(true);

                var b = p.AsQueryable().FirstOrDefault(t => t.Id == a.Id);
                Assert.IsNotNull(b);
                Assert.IsTrue(b.LastName == "Hoang");
            }
        }

        [TestMethod]
        public async Task Test_Delete_Person()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(databaseName: TestDbName)
                .Options;

            using (var factory = new TestFactory(new TestDbContext(options), true))
            {
                var p = factory.For<Person>();
                p.DeleteAll("Duy");
                await factory.SaveAsync();

                Assert.IsTrue(p.AsQueryable().Count() == 0);

                p.Add(new Person { FirstName = "Duy" }, "Duy");
                await factory.SaveAsync();

                var a = p.AsQueryable().FirstOrDefault();
                Assert.IsNotNull(a);
                Assert.IsNull(a.LastName);

                p.Delete(a, "Duy");
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

            var factory1 = new TestFactory(new TestDbContext(options), true);
            var factory2 = new TestFactory(new TestDbContext(options), true);

            var p1 = factory1.For<Person>();
            var p2 = factory2.For<Person>();

            //Clean Db
            p1.DeleteAll("Duy");
            factory1.Save();

            p1.Add(new Person { FirstName = "Duy" }, "Duy");
            factory1.Save();

            var ps1 = p1.AsQueryable().AsNoTracking().First();
            var ps2 = p2.AsQueryable().AsNoTracking().First();

            ps2.LastName = "Hoang";
            p2.Update(ps2, "Duy");
            factory2.Save();

            ps1.LastName = "Bao";
            p1.Update(ps1, "Duy");
            factory1.Save();

            Assert.IsNotNull(ps1.RowVersion);
            Assert.IsNotNull(ps2.RowVersion);
            Assert.AreNotEqual(ps1.RowVersion, ps2.RowVersion);
        }

        [TestMethod]
        [ExpectedException(typeof(InvaidKeysException))]
        public void Delete_With_InvalidKeys()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
              .UseInMemoryDatabase(TestDbName)
              .Options;

            var factory = new TestFactory(new TestDbContext(options), true);
            factory.For<Person>().DeleteByKey(0);
        }

        [TestMethod]
        [ExpectedException(typeof(InvaidKeysException))]
        public async Task DeleteAsync_With_InvalidKeys()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
              .UseInMemoryDatabase(TestDbName)
              .Options;

            var factory = new TestFactory(new TestDbContext(options), true);
            await factory.For<Person>().DeleteByKeyAsync(0);
        }
    }
}