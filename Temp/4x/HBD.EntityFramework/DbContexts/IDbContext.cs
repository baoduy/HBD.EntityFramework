using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Threading;
using System.Threading.Tasks;

namespace System.Data.Entity
{
    public interface IDbContext : IDisposable
    {
        DbChangeTracker ChangeTracker { get; }
        DbContextConfiguration Configuration { get; }
        Database Database { get; }

        DbEntityEntry Entry(object entity);

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        // Summary:
        //     Validates tracked entities and returns a Collection of System.Data.Entity.Validation.DbEntityValidationResult
        //     containing validation results.
        //
        // Returns:
        //     Collection of validation results for invalid entities. The collection is never
        //     null and must not contain null values or results for valid entities.
        //
        // Remarks:
        //     1. This method calls DetectChanges() to determine states of the tracked entities
        //     unless DbContextConfiguration.AutoDetectChangesEnabled is set to false. 2. By
        //     default only Added on Modified entities are validated. The user is able to change
        //     this behavior by overriding ShouldValidateEntity method.
        IEnumerable<DbEntityValidationResult> GetValidationErrors();

        int SaveChanges();

        Task<int> SaveChangesAsync();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        DbSet Set(Type entityType);

        //DbEntityValidationResult ValidateEntity(DbEntityEntry entityEntry, IDictionary<object, object> items);
    }
}