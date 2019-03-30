using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ConditionalValidation.Interface
{
    public interface IRuleBuilder<T>
    {
        IRuleBuilder<T> AddRule(string propertyName, Expression<Func<T, bool>> expression);
    }
}
