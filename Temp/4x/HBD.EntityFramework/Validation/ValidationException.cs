using HBD.Framework.Core;
using System;
using System.Collections.Generic;

namespace HBD.EntityFramework.Validation
{
    public class ValidationException : Exception
    {
        public IList<ValidationResult> ValidationResult { get; private set; }

        public ValidationException(IList<ValidationResult> validationResult)
            : base("The validation is failed.")
        {
            Guard.CollectionMustNotEmpty(validationResult, nameof(validationResult));
            this.ValidationResult = validationResult;
        }
    }

    public class ValidationException<T> : Exception
    {
        public IList<ValidationResult<T>> ValidationResult { get; private set; }

        public ValidationException(IList<ValidationResult<T>> validationResult)
            : base("The validation is failed.")
        {
            Guard.CollectionMustNotEmpty(validationResult, nameof(validationResult));
            this.ValidationResult = validationResult;
        }
    }
}