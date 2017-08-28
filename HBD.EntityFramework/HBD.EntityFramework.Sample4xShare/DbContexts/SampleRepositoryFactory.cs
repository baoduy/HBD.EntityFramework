using HBD.EntityFramework.Core;
using HBD.EntityFramework.DbContexts.DbRepositories;
using System.ComponentModel.Composition;
using System.Data.Entity;
using HBD.EntityFramework.DbContexts;

namespace HBD.EntityFramework.Sample.DbContexts
{
    [Export]
    [Export(typeof(IDbRepositoryFactory))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class SampleRepositoryFactory : DbRepositoryFactory
    {
        [ImportingConstructor]
        public SampleRepositoryFactory(IDbContext context) : base(context, false)
        {
        }

        public override bool IsApplyFullValidation => true;
    }
}