#if NETSTANDARD2_0
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Threading.Tasks;

namespace HBD.EntityFramework.DbContexts
{
    /// <summary>
    /// This EntityDbContext will scan all IEntityMappingConfiguration<TEntity> classes and import into ModelBuilder automatically.
    /// </summary>
    public class EntityDbContext : DbContext, IDbContext
    {
        public EntityDbContext(DbContextOptions options)
            : base(options)
        { }

        public Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess)
            => base.SaveChangesAsync(acceptAllChangesOnSuccess, new System.Threading.CancellationToken());

        public Task<int> SaveChangesAsync()
            => base.SaveChangesAsync(new System.Threading.CancellationToken());

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.RegisterMappingFromAssembly(GetType().GetTypeInfo().Assembly);
        }
    }
}
#endif