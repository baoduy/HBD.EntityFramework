using HBD.EntityFramework.DbContexts;
using System.ComponentModel.Composition;


namespace HBD.EntityFramework.Sample.DbContexts
{
    [Export]
    [Export(typeof(IDbContext))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class SampleDbContext : EntityDbContext
    {
        public SampleDbContext()
            : base("SampleDbConnectionString")
        { }

    }
}