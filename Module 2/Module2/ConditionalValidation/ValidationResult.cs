using System.Collections.Generic;

namespace ConditionalValidation
{
    public class ValidationResult
    {
        public virtual bool IsValid => this.Errors.Count == 0;
        
        public IList<ValidationFailure> Errors { get; }

        public ValidationResult()
        {
            this.Errors = new List<ValidationFailure>();
        }
    }
}
