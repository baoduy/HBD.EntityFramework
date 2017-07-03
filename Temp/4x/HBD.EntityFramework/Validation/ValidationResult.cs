using System.Collections.Generic;

namespace HBD.EntityFramework.Validation
{
    public class ValidationResult
    {
        public ValidationResult(object entity, ValidateState state)
        {
            this.Entity = entity;
            this.State = state;
        }

        public virtual object Entity { get; private set; }
        public virtual ValidateState State { get; private set; }
        public bool IsValid { get { return this.ValidationErrors.Count == 0; } }
        public virtual IList<ValidationError> ValidationErrors { get; } = new List<ValidationError>();
    }

    public class ValidationResult<T> : ValidationResult
    {
        public ValidationResult(T entity, ValidateState state)
            : base(entity, state) { }

        public new virtual T Entity { get { return (T)base.Entity; } }
    }
}