using HBD.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Composition;



namespace HBD.EntityFramework.Sample.DbContexts
{
    [Export]
    [Export(typeof(DbContext))]
    public class SampleDbContext : EntityDbContext
    {
        [ImportingConstructor]
        public SampleDbContext(DbContextOptions<SampleDbContext> options)
            : base(options)
        { }
    }
}