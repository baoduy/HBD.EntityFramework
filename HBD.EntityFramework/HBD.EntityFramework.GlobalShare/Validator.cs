using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HBD.EntityFramework
{
    /// <summary>
    /// The wrapper of DataAnnotations.Validator
    /// </summary>
    public static class Validator
    {
        public static bool TryValidateObject(object instance, ICollection<ValidationResult> validationResults)
            => System.ComponentModel.DataAnnotations.Validator.TryValidateObject(instance, new ValidationContext(instance), validationResults, true);

        public static void ValidateObject(object instance)
              => System.ComponentModel.DataAnnotations.Validator.ValidateObject(instance, new ValidationContext(instance), true);
    }
}