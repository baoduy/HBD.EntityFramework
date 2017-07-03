using HBD.EntityFramework.Test.TestObjects;
using HBD.QueryBuilders;
using HBD.QueryBuilders.Context;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace HBD.EntityFramework.Test.DbContexts
{
    [TestClass]
    public class AuditTrailDbContextTestCases
    {
        [TestMethod]
        [TestCategory("Entity.DbContexts")]
        public void Can_TestDbContext_CreateDb()
        {
            using (var con = new TestDbContext())
            {
                //Drop the database if existed
                if (con.Database.Exists()) con.Database.Delete();
                Assert.IsFalse(con.Database.Exists());

                //-Re-Create DB will execute automatically
                con.Database.Initialize(true);

                using (var query = new SqlQueryBuilderContext(con.Database.Connection))
                {
                    var select = query.CreateSelectQuery();
                    select.Fields(f => f.Count()).From("Audit.ActionType");
                    Assert.IsTrue((int)query.ExecuteScalar(select) > 0);
                }

                //Drop DataBase
                if (con.Database.Exists()) con.Database.Delete();
                Assert.IsFalse(con.Database.Exists());
            }
        }
    }
}