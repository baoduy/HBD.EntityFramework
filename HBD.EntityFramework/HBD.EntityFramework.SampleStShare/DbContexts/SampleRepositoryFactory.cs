using HBD.EntityFramework.DbContexts.DbRepositories;
using HBD.EntityFramework.DbContexts.Interfaces;
using System.Composition;
using Microsoft.EntityFrameworkCore;

namespace HBD.EntityFramework.Sample.DbContexts
{
    [Export]
    [Export(typeof(IDbRepositoryFactory))]
    public class SampleRepositoryFactory : DbRepositoryFactory
    {
        [ImportingConstructor]
        public SampleRepositoryFactory(DbContext context) : base(context, true)
        {
        }

        public override bool IsApplyFullValidation => true;

        protected override IDbRepository<TEntity, int, string> CreateRepository<TEntity>() 
            => new SampleRepository<TEntity>(this, this.DbContext);
    }
}