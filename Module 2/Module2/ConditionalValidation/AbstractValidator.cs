using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ConditionalValidation.Infastructure;
using ConditionalValidation.Interface;

namespace ConditionalValidation
{
    public abstract class AbstractValidator<T> : IRuleBuilder<T>
    {
        private List<IValidationRule<T>> Rules { get; } = 
            new List<IValidationRule<T>>();

        public Expression ConvertAllRulesToExpression()
        {
            var exprs = Rules.Select(rule => rule.Condition);
            return exprs.Aggregate((expr1, expr2) => expr1.And(expr2));
        }

        public IRuleBuilder<T> AddRule(string propertyName, Expression<Func<T, bool>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }
            var validationRule = new ValidationRule<T>(propertyName, expression);
            this.Rules.Add(validationRule);
            return this;
        }

        public virtual ValidationResult Validate(T model)
        {
            return Validate(new ValidationContext<T>(model));
        }

        public virtual ValidationResult Validate(ValidationContext<T> context)
        {
            context.Guard("Cannot pass null to Validate.", nameof(context));
            context.InstanceToValidate.Guard("Cannot pass null model to Validate.", 
                nameof(context.InstanceToValidate));
            
            var result = new ValidationResult();

            foreach (var validationFailure in Rules.SelectMany(rule => rule.Validate(context)))
                result.Errors.Add(validationFailure);
            
            return result;
        }
    }
}
