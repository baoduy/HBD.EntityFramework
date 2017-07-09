using HBD.EntityFramework.DbContexts.DbRepositories;
using HBD.EntityFramework.DbContexts.Interfaces;
using System.Composition;
using Microsoft.EntityFrameworkCore;

namespace HBD.EntityFramework.Sample.DbContexts
{
    [Export]
    [Export(typeof(IDbRepoFactory))]
    public class SampleRepositoryFactory : DbRepoFactory
    {
        [ImportingConstructor]
        public SampleRepositoryFactory(DbContext context) : base(context, true)
        {
        }

        public override bool IsApplyFullValidation => true;

        protected override IDbRepo<TEntity> CreateRepo<TEntity>() 
            => new SampleRepository<TEntity>(this, this.DbContext);
    }
}