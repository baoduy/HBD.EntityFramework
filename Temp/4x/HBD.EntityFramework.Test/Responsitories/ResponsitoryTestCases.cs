using HBD.EntityFramework.AuditTrail.Entities;
using HBD.EntityFramework.Test.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace HBD.EntityFramework.Test.Responsitories
{
    [TestClass]
    public class ResponsitoryTestCases
    {
        [TestInitialize]
        public void Ininitializer()
        {
            using (var con = new TestDbContext())
            {
                //Drop DataBase
                if (con.Database.Exists())
                    con.Database.Delete();

                con.Database.Initialize(false);
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            using (var con = new TestDbContext())
            {
                //Drop DataBase
                if (con.Database.Exists())
                    con.Database.Delete();
            }
        }
    }
}