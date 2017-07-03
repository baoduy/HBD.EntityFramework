using HBD.EntityFramework.Repositories;
using HBD.Framework.St.Tests.TestObjs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace HBD.Framework.St.Tests.Repositories
{
    [TestClass]
    public class EntityRepositoryTests
    {
        public const string TestDbName = "Test_Database";

        [TestMethod]
        public void Test_AutoDispose_AutoDisposeDbContext_DbContext()
        {
            var m = new Mock<DbContext>();
            m.Protected().Setup("Dispose", true).Verifiable();

            var factory = new EntityRepositoryFactory(m.Object, true);
            factory.Dispose();

            m.Protected().Verify("Dispose", Times.Once(), true);
        }

        [TestMethod]
        public void Test_AutoDispose_NotAutoDisposeDbContext_DbContext()
        {
            var m = new Mock<DbContext>();
            m.Protected().Setup("Dispose", true).Verifiable();

            var factory = new EntityRepositoryFactory(m.Object, false);
            factory.Dispose();

            m.Protected().Verify("Dispose", Times.Never(), true);
        }

        //[TestMethod]
        //public void Test_Create_Person()
        //{
        //    var options = new DbContextOptionsBuilder<TestDbContext>()
        //        .UseInMemoryDatabase(databaseName: TestDbName)
        //        .Options;

        //    using (var factory = new EntityRepositoryFactory(new TestDbContext(options), true))
        //    {
        //        var p = factory.RepositoryFor<Person>();
        //        p.Add(new Person { FirstName = "Duy" }, "Duy");

        //        Assert.IsTrue(factory.Save() == 1);

        //        Assert.IsTrue(p.All().FirstOrDefault()?.FirstName == "Duy");
        //    }
        //}

        //[TestMethod]
        //public void Test_Select_NoPerson()
        //{
        //    var options = new DbContextOptionsBuilder<TestDbContext>()
        //        .UseInMemoryDatabase(databaseName: "New_Database")
        //        .Options;

        //    using (var factory = new EntityRepositoryFactory(new TestDbContext(options), true))
        //    {
        //        var p = factory.RepositoryFor<Person>();
        //        Assert.IsFalse(p.AsQueryable().Any());
        //    }
        //}

        //[TestMethod]
        //public async Task Test_Update_Person()
        //{
        //    var options = new DbContextOptionsBuilder<TestDbContext>()
        //        .UseInMemoryDatabase(databaseName: TestDbName)
        //        .Options;

        //    using (var factory = new EntityRepositoryFactory(new TestDbContext(options), true))
        //    {
        //        var p = factory.RepositoryFor<Person>();
        //        p.Add(new Person { FirstName = "Duy" }, "Duy");
        //        await factory.SaveAsync();

        //        var a = p.AsQueryable().FirstOrDefault(t => t.Id == 1);
        //        Assert.IsNotNull(a);
        //        Assert.IsNull(a.LastName);

        //        a.LastName = "Hoang";
        //        p.Update(a, "Duy");
        //        await factory.SaveAsync(true);

        //        var b = p.AsQueryable().FirstOrDefault(t => t.Id == 1);
        //        Assert.IsNotNull(b);
        //        Assert.IsTrue(b.LastName=="Hoang");
        //    }
        //}

        //[TestMethod]
        //public async Task Test_Update_Deattached_Person()
        //{
        //    var options = new DbContextOptionsBuilder<TestDbContext>()
        //        .UseInMemoryDatabase(databaseName: TestDbName)
        //        .Options;

        //    using (var factory = new EntityRepositoryFactory(new TestDbContext(options), true))
        //    {
        //        var p = factory.RepositoryFor<Person>();

        //        p.DeleteAll("Duy");
        //        await factory.SaveAsync();

        //        p.Add(new Person { FirstName = "Duy" }, "Duy");
        //        await factory.SaveAsync();

        //        var a = p.AsQueryable().AsNoTracking().FirstOrDefault();
        //        Assert.IsNotNull(a);
        //        Assert.IsNull(a.LastName);

        //        a.LastName = "Hoang";
        //        p.Update(a, "Duy");
        //        await factory.SaveAsync(true);

        //        var b = p.AsQueryable().FirstOrDefault(t => t.Id == a.Id);
        //        Assert.IsNotNull(b);
        //        Assert.IsTrue(b.LastName == "Hoang");
        //    }
        //}

        //[TestMethod]
        //public async Task Test_Delete_Person()
        //{
        //    var options = new DbContextOptionsBuilder<TestDbContext>()
        //        .UseInMemoryDatabase(databaseName: TestDbName)
        //        .Options;

        //    using (var factory = new EntityRepositoryFactory(new TestDbContext(options), true))
        //    {
        //        var p = factory.RepositoryFor<Person>();
        //        p.DeleteAll("Duy");
        //        await factory.SaveAsync();

        //        Assert.IsTrue(p.Count() == 0);

        //        p.Add(new Person { FirstName = "Duy" }, "Duy");
        //        await factory.SaveAsync();

        //        var a = p.AsQueryable().FirstOrDefault();
        //        Assert.IsNotNull(a);
        //        Assert.IsNull(a.LastName);

        //        p.Delete(a,"Duy");
        //        await factory.SaveAsync(true);

        //        var b = p.AsQueryable().FirstOrDefault(t => t.Id == a.Id);
        //        Assert.IsNull(b);
        //    }
        //}
    }
}
