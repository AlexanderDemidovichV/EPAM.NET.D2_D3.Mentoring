namespace ConditionalValidation
{
    public class ValidationFailure
    {
        public ValidationFailure(string propertyName, string errorMessage)
        {
            this.PropertyName = propertyName;
            this.ErrorMessage = errorMessage;
        }
        
        public string PropertyName { get; set; }
        
        public string ErrorMessage { get; set; }
    }
}
