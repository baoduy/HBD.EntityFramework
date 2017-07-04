using HBD.EntityFramework.DbContexts.DbRepositories;
using HBD.EntityFramework.DbContexts.Interfaces;
using System.ComponentModel.Composition;
using System.Data.Entity;

namespace HBD.EntityFramework.Sample.DbContexts
{
    [Export]
    [Export(typeof(IDbRepositoryFactory))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class SampleRepositoryFactory : DbRepositoryFactory
    {
        [ImportingConstructor]
        public SampleRepositoryFactory(DbContext context) : base(context, false)
        {
        }

        public override bool IsApplyFullValidation => true;

        protected override IDbRepository<TEntity, int, string> CreateRepository<TEntity>() 
            => new SampleRepository<TEntity>(this, this.DbContext);
    }
}