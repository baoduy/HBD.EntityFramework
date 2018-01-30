using System.Composition;
using HBD.EntityFramework.Core;
using HBD.EntityFramework.DbContexts;
using HBD.EntityFramework.DbContexts.DbRepositories;

namespace HBD.EntityFramework.Sample.DbContexts
{
    [Export]
    [Export(typeof(IDbFactory))]
    public class SampleRepositoryFactory : DbFactory
    {
        [ImportingConstructor]
        public SampleRepositoryFactory(IDbContext context) : base(context, true)
        {
        }

        public override bool IsApplyFullValidation => true;
    }
}