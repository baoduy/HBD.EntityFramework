namespace HBD.EntityFramework.Validation
{
    public class ValidationError
    {
        public ValidationError(string propertyName, string errorMessage)
        {
            this.PropertyName = propertyName;
            this.ErrorMessage = errorMessage;
        }

        public string PropertyName { get; set; }
        public string ErrorMessage { get; set; }
    }
}