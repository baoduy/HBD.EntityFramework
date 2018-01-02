using System.ComponentModel.Composition;
using HBD.EntityFramework.DbContexts;

namespace HBD.EntityFramework.TestDal
{
    [Export(typeof(ITestDbContext))]
    [Export(typeof(IDbContext))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    internal class InternalDbContext: TestDbContext, IDbContext
    {
        [ImportingConstructor]
        public InternalDbContext() : base("Data Source=TestDb.sdf;")
        {
        }
    }
}
