using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HBD.EntityFramework.Base
{
    public abstract class Entity<TKeyType> : IEntity
    {
        protected Entity()
        {
            this.UpdateAuditPropertyMode = UpdateAuditPropertyMode.Auto;
        }

        [Required]
        [Key]
        public virtual TKeyType Id { get; set; }

        #region Audit Properties

        [SkipTracking]
        [StringLength(256)]
        [Required]
        public string CreatedBy { get; set; }

        [SkipTracking]
        [Required]
        public DateTime CreatedTime { get; set; }

        [SkipTracking]
        [StringLength(256)]
        public string UpdatedBy { get; set; }

        [SkipTracking]
        public DateTime? UpdatedTime { get; set; }

        /// <summary>
        /// Auto: The CreatedBy, CreatedTime, UpdatedBy and UpdatedTime will be apply automatically when SaveChanges.
        /// Manual: All Audit Properties above must be assigned before SaveChanges.
        /// </summary>
        [SkipTracking]
        [NotMapped]
        public UpdateAuditPropertyMode UpdateAuditPropertyMode { get; set; }

        #endregion Audit Properties

        public virtual bool IsDisabled { get; set; } = false;

        [Timestamp]
        [SkipTracking]
        public virtual byte[] Version { get; internal set; }
    }

    public enum UpdateAuditPropertyMode
    { Auto, Manual }
}