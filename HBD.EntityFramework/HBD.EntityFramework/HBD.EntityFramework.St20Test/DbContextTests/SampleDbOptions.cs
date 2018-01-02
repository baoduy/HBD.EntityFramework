
using HBD.EntityFramework.Sample.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Composition;

namespace HBD.EntityFramework.TestSample.DbContextTests
{
    [Export]
    public class SampleDbOptionsFactory
    {
        [Export(typeof(DbContextOptions<SampleDbContext>))]
        public DbContextOptions<SampleDbContext> SampleDbOptions
            => new DbContextOptionsBuilder<SampleDbContext>()
                .UseSqlite("Data Source=SampleDb.db")
                .Options;
    }
}