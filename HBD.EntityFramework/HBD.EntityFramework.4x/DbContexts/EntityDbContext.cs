using System.Data.Entity;
using System.Reflection;

namespace HBD.EntityFramework.DbContexts
{
    /// <summary>
    /// This EntityDbContext will scan all IEntityMappingConfiguration<TEntity> classes and import into ModelBuilder automatically.
    /// </summary>
    public class EntityDbContext : DbContext
    {
        public EntityDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.RegisterMappingFromAssembly(this.GetType().GetTypeInfo().Assembly);
        }
    }
}