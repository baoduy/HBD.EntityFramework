using HBD.EntityFramework.Core;
using HBD.EntityFramework.DbContexts.DbRepositories;
using System.ComponentModel.Composition;
using HBD.EntityFramework.DbContexts;

namespace HBD.EntityFramework.Sample.DbContexts
{
    [Export]
    [Export(typeof(IDbFactory))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class SampleRepositoryFactory : DbFactory
    {
        [ImportingConstructor]
        public SampleRepositoryFactory(IDbContext context) : base(context, false)
        {
        }

        public override bool IsApplyFullValidation => true;
    }
}