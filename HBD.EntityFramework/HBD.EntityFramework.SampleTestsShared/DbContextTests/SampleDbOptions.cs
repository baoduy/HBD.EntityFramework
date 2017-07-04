#if NETSTANDARD2_0 || NETSTANDARD1_6 || NETCOREAPP1_1 || NETCOREAPP2_0
using HBD.EntityFramework.Sample.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Composition;
#else
using System.ComponentModel.Composition;
#endif



namespace HBD.EntityFramework.TestSample.DbContextTests
{
    [Export]
    public class SampleDbOptionsFactory
    {

#if NETSTANDARD2_0 || NETSTANDARD1_6 || NETCOREAPP1_1 || NETCOREAPP2_0
        [Export(typeof(DbContextOptions<SampleDbContext>))]
        public DbContextOptions<SampleDbContext> SampleDbOptions
            => new DbContextOptionsBuilder<SampleDbContext>()
                .UseSqlite("Data Source=SampleDb.db")
                //.UseSqlServer("Data Source=WIN-HFIQC0VT0PC\\SQLEXPRESS;Initial Catalog=SampleDb;Integrated Security=True")
                //.ConfigureWarnings(warnings => warnings.Ignore(CoreEventId.IncludeIgnoredWarning))
                .Options;
#else
        [Export("SampleDbConnectionString", typeof(string))]
        public string SampleDbOptions
          => "Data Source=WIN-HFIQC0VT0PC\\SQLEXPRESS;Initial Catalog=SampleDb;Integrated Security=True";
#endif

    }
}