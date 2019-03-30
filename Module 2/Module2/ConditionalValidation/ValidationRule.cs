using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using ConditionalValidation.Interface;

namespace ConditionalValidation
{
    public class ValidationRule<T>: IValidationRule<T>
    {
        public ValidationRule(string propertyName, Expression<Func<T, bool>> condition)
        {
            this.PropertyName = propertyName;
            this.Condition = condition;
        }

        public string PropertyName { get; set; }

        public Expression<Func<T, bool>> Condition { get; set; }

        public IEnumerable<ValidationFailure> Validate(ValidationContext<T> context)
        {
            var condition = Condition.Compile()(context.InstanceToValidate);
            
            var validationFailures = new List<ValidationFailure>();
            if (!condition)
            {
                validationFailures.Add(new ValidationFailure(PropertyName, "error message"));
            }

            return validationFailures;
        }
    }
}
