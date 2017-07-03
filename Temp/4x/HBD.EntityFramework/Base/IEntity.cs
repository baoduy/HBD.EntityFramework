using System;

namespace HBD.EntityFramework.Base
{
    public interface IEntity
    {
        string CreatedBy { get; set; }
        DateTime CreatedTime { get; set; }
        string UpdatedBy { get; set; }
        DateTime? UpdatedTime { get; set; }
        UpdateAuditPropertyMode UpdateAuditPropertyMode { get; set; }
    }
}