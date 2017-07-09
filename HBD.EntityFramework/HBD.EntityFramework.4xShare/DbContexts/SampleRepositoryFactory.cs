using HBD.EntityFramework.DbContexts.DbRepositories;
using HBD.EntityFramework.DbContexts.Interfaces;
using System.ComponentModel.Composition;
using System.Data.Entity;

namespace HBD.EntityFramework.Sample.DbContexts
{
    [Export]
    [Export(typeof(IDbRepoFactory))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class SampleRepositoryFactory : DbRepoFactory
    {
        [ImportingConstructor]
        public SampleRepositoryFactory(DbContext context) : base(context, false)
        {
        }

        public override bool IsApplyFullValidation => true;

        protected override IDbRepo<TEntity> CreateRepo<TEntity>() 
            => new SampleRepository<TEntity>(this, this.DbContext);
    }
}