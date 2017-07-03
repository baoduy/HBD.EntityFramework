using HBD.EntityFramework.AuditTrail.Entities;
using System.Data.Entity;

namespace HBD.EntityFramework.DbContexts
{
    /// <summary>
    /// We are facing the issue of EF6 when the DbContext create the DB at the first time. The parent initialize won't be executed.
    /// So all initial static data of parent DbContext won't be created.
    /// The solution is we create the generic initialize to make it inheritable for children classes. So when initial the child dbcontext the parent will also initialed.
    /// </summary>
    /// <typeparam name="TDbContext">The child DbContext type.</typeparam>
    public class CreateAuditTrailDbIfNotExists<TDbContext> : CreateDatabaseIfNotExists<TDbContext> where TDbContext : AuditTrailDbContext
    {
        protected override void Seed(TDbContext context)
        {
            base.Seed(context);

            context.AuditTrailActionTypes.AddRange(
               new[]{
                        new AuditTrailActionType
                        {
                            Id = (int)EntityState.Added,
                            ActionName = EntityState.Added.ToString()
                        },
                        new AuditTrailActionType
                        {
                            Id = (int)EntityState.Modified,
                            ActionName = EntityState.Modified.ToString()
                        },
                        new AuditTrailActionType
                        {
                            Id = (int)EntityState.Deleted,
                            ActionName = EntityState.Deleted.ToString()
                        }
               });
        }
    }
}