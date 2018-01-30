using System.ComponentModel.Composition;
using HBD.EntityFramework.Core;
using HBD.EntityFramework.DbContexts;

namespace HBD.EntityFramework.TestDal
{
    [Export(typeof(IDbFactory))]
    [Export()]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    internal class TestDbFactory:HBD.EntityFramework.DbContexts.DbRepositories.DbFactory
    {
        [ImportingConstructor]
        public TestDbFactory(IDbContext dbContext) : base(dbContext, true)
        {
        }
    }
}
