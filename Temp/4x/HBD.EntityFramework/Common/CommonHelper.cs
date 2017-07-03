using EntityFramework.MappingAPI.Extensions;
using HBD.EntityFramework.AuditTrail.Entities;
using HBD.EntityFramework.Base;
using HBD.Framework;
using HBD.Framework.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Utilities;
using System.Linq;

namespace HBD.EntityFramework.Common
{
    public static partial class CommonHelper
    {
        internal static void ApplyAuditProperties(IEntity entity, EntityState state)
        {
            if (entity.UpdateAuditPropertyMode == UpdateAuditPropertyMode.Manual)
                return;

            switch (state)
            {
                case EntityState.Added:
                    {
                        entity.CreatedBy = UserPrincipalHelper.UserName;
                        entity.CreatedTime = DateTime.Now;
                    }
                    break;

                case EntityState.Modified:
                    {
                        if (string.IsNullOrWhiteSpace(entity.CreatedBy))
                            entity.CreatedBy = UserPrincipalHelper.UserName;
                        if (entity.CreatedTime == DateTime.MinValue)
                            entity.CreatedTime = DateTime.Now;

                        entity.UpdatedBy = UserPrincipalHelper.UserName;
                        entity.UpdatedTime = DateTime.Now;
                    }
                    break;
            }
        }

        internal static bool IsTrackable(DbEntityEntry<IEntity> entity)
        {
            //Check State
            if (entity.State != EntityState.Modified
                && entity.State != EntityState.Deleted)
            {
                return false;
            }

            //Check Tracking Attribute
            if (!entity.Entity.HasAttribute<TrackChangesAttribute>())
                return false;

            return true;
        }

        internal static AuditTrailLog GetAuditLog(DbEntityEntry<IEntity> entity, DbContext context)
        {
            var dbInfo = context.Db(entity.Entity.GetType().GetEntityType());

            var log = new AuditTrailLog
            {
                ActionType = entity.State,
                AuditDate = entity.Entity.UpdatedTime.Value,
                ResordId = string.Join(",", entity.Entity.GetValues(dbInfo.Pks).ToArray()),
                TableName = dbInfo.TableName,
                UserName = entity.Entity.UpdatedBy
            };

            foreach (var d in GetAuditDetails(entity))
                log.Details.Add(d);

            return log;
        }

        internal static IEnumerable<AuditTrailDetails> GetAuditDetails(DbEntityEntry<IEntity> entity)
        {
            foreach (var propName in entity.CurrentValues.PropertyNames)
            {
                //Check Ignore Property
                if (entity.Entity.HasAttributeOnProperty<SkipTrackingAttribute>(propName)) continue;

                var curr = entity.CurrentValues[propName];
                var orig = entity.OriginalValues[propName];

                if (curr == null && orig == null)
                {
                    continue;
                }
                if (curr != null && orig != null)
                {
                    if (curr.Equals(orig))
                        continue;
                }

                yield return new AuditTrailDetails
                {
                    ColumnName = propName,
                    NewValue = curr?.ToString() ?? string.Empty,
                    OldValue = orig?.ToString() ?? string.Empty
                };
            }
        }
    }
}