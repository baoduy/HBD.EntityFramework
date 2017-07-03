using Autofac;
using HBD.EntityFramework.UnitOfWorks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace HBD.EntityFramework.Test.UnitOfWorks
{
    [TestClass]
    public class UnitOfWorksTestCases
    {
        public class TestEntity { }

        IList<TestEntity> listData = new List<TestEntity>
            {
                new TestEntity(),
                new TestEntity(),
                new TestEntity(),
                new TestEntity(),
                new TestEntity(),
            };

        [TestInitialize]
        public void Initializer()
        {
            var data = listData.AsQueryable();

            var db = new Mock<IDbContext>();
            db.Setup(d => d.Set<TestEntity>()).Returns(() =>
            {
                var entitySet = new Mock<DbSet<TestEntity>>();
                //Get Data
                entitySet.As<IQueryable<TestEntity>>().Setup(a => a.Provider).Returns(data.Provider);
                entitySet.As<IQueryable<TestEntity>>().Setup(a => a.Expression).Returns(data.Expression);
                entitySet.As<IQueryable<TestEntity>>().Setup(a => a.ElementType).Returns(data.ElementType);
                entitySet.As<IQueryable<TestEntity>>().Setup(a => a.GetEnumerator()).Returns(data.GetEnumerator());

                //Add, Delete entity
                entitySet.Setup(e => e.Add(It.IsAny<TestEntity>()))
                    .Callback<TestEntity>((entity) => listData.Add(entity));
                entitySet.Setup(e => e.Remove(It.IsAny<TestEntity>()))
                   .Callback<TestEntity>((entity) => listData.Remove(entity));

                return entitySet.Object;
            });

            db.Setup(d => d.Entry(It.IsAny<TestEntity>()))
                .Returns(()=> { return null; });

            var m = new ContainerBuilder();
            m.RegisterInstance(db.Object)
                .As<IDbContext>()
                .SingleInstance();

            m.RegisterType<DbContextUnitOfWork<IDbContext>>().As<IUnitOfWork>();

            IocManager.Update(m);
        }

        [TestMethod]
        [TestCategory("Entity.UnitOfWorks")]
        public void Can_Load_UnitOfWork()
        {
            var unit = IocManager.Default.Resolve<IUnitOfWork>();
            Assert.IsNotNull(unit);

            var r1 = unit.Responsitory<TestEntity>();
            Assert.IsNotNull(r1);

            var obj = r1.GetAll();
            Assert.IsNotNull(obj);

            //Count Test
            var firstCount = obj.Count();
            Assert.IsTrue(firstCount == listData.Count);

            //Add Test
            var firstObj = new TestEntity();
            r1.Add(firstObj);
            var secondCount = obj.Count();
            Assert.IsTrue(secondCount == listData.Count);
            Assert.IsTrue(secondCount > firstCount);

            //Delete Test
            r1.Delete(firstObj);
            var thirdCount = obj.Count();
            Assert.IsTrue(thirdCount == listData.Count);
            Assert.IsTrue(thirdCount == firstCount);

            //Update Test
            Assert.IsFalse(r1.Update(listData[0]));//because DbEntityEntry is Null.
        }
    }
}
