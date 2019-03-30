using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ConditionalValidation.Interface
{
    public interface IValidationRule<T>
    {
        Expression<Func<T, bool>> Condition { get; set; }
        IEnumerable<ValidationFailure> Validate(ValidationContext<T> context);
    }
}
