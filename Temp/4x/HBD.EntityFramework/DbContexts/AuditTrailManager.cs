using EntityFramework.MappingAPI.Extensions;
using HBD.EntityFramework.AuditTrail.Entities;
using HBD.Framework.Core;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace HBD.EntityFramework.DbContexts
{
    public class AuditTrailManager
    {
        private readonly AuditTrailDbContext _context;

        internal AuditTrailManager(AuditTrailDbContext context)
        {
            Guard.ArgumentIsNotNull(context, "DbContext");
            this._context = context;
        }

        public IQueryable<AuditTrailLog> GetAuditTrail<TEntity>() where TEntity : class
        {
            var dbInfo = this._context.Db<TEntity>();
            return this._context.AuditTrailLogs.Where(a => a.TableName == dbInfo.TableName);
        }

        public IQueryable<AuditTrailLog> GetAuditTrail<TEntity>(Expression<Func<TEntity, object>> keySelector) where TEntity : class
        {
            var colName = this._context.Db<TEntity>().Prop(keySelector);
            var query = this.GetAuditTrail<TEntity>().Where(a => a.Details.Any(d => d.ColumnName == colName.PropertyName));
            return query;
        }
    }
}