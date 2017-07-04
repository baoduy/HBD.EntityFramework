using HBD.EntityFramework.DbContexts;
using System.ComponentModel.Composition;
using System.Data.Entity;


namespace HBD.EntityFramework.Sample.DbContexts
{
    [Export]
    [Export(typeof(DbContext))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class SampleDbContext : EntityDbContext
    {
        [ImportingConstructor]
        public SampleDbContext([Import("SampleDbConnectionString")] string nameOrConnectionString)
            : base(nameOrConnectionString)
        { }

    }
}