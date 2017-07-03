namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// The SkipTracking to mark the property that will be ignore when AuditTrailDbContext extract the audit trail information.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SkipTrackingAttribute : Attribute { }
}