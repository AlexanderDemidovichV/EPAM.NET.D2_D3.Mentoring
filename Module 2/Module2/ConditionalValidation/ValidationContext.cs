namespace ConditionalValidation
{
    public class ValidationContext<T>
    {
        public ValidationContext(T instanceToValidate)
           
        {
            this.InstanceToValidate = instanceToValidate;
        }
        
        public T InstanceToValidate { get; }
    }
}
