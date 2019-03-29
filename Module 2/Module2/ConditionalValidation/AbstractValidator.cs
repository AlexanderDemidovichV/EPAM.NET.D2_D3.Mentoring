using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ConditionalValidation.Interface;

namespace ConditionalValidation
{
    public abstract class AbstractValidator<T> : IRuleBuilder
    {
        internal List<Expression<Func<T, bool>>> Rules { get; } = 
            new List<Expression<Func<T, bool>>>();

        public IRuleBuilder AddRule(Expression<Func<T, bool>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            this.Rules.Add(expression);
            return this;
        }
    }
}
