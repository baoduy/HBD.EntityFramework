using HBD.EntityFramework.Base;

namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// The StringLength attribute to check the DbValue length for SecuredString.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CryptionStringLengthAttribute : StringLengthAttribute
    {
        public CryptionStringLengthAttribute(int maximumLength) : base(maximumLength)
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var checkValue = string.Empty;
            if (value != null)
            {
                checkValue = value.GetType() == typeof(CryptionString) ? ((CryptionString)value).DbValue : value.ToString();
            }
            return base.IsValid(checkValue, validationContext);
        }
    }
}