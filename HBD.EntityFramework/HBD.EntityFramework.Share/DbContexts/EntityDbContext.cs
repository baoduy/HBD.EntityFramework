using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace HBD.EntityFramework.DbContexts
{
    /// <summary>
    /// This EntityDbContext will scan all IEntityMappingConfiguration<TEntity> classes and import into ModelBuilder automatically.
    /// </summary>
    public class EntityDbContext : DbContext
    {
        public EntityDbContext(DbContextOptions options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.RegisterMappingFromAssembly(this.GetType().GetTypeInfo().Assembly);
        }
    }
}