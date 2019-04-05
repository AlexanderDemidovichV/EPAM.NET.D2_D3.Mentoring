using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Mapper
{
    internal static class BindingHelper
    {
        internal static List<MemberBinding> MapProperties(this List<MemberBinding> bindings, 
            ParameterExpression parameter, Type source, Type destination)
        {
            bindings.AddRange(
                from sourceProperty in source.GetProperties() 
                let targetProperty = destination.GetProperty(sourceProperty.Name) 
                where targetProperty != null 
                where targetProperty.CanWrite 
                where targetProperty.PropertyType.IsAssignableFrom(sourceProperty.PropertyType) 
                select Expression.Bind(targetProperty, Expression.Property(parameter, sourceProperty)));
            return bindings;
        }

        internal static List<MemberBinding> MapFields(this List<MemberBinding> bindings, 
            ParameterExpression parameter, Type source, Type destination)
        {
            bindings.AddRange(
                from sourceField in source.GetFields() 
                let targetField = destination.GetField(sourceField.Name) 
                where targetField != null 
                where !targetField.IsPrivate 
                where targetField.FieldType.IsAssignableFrom(sourceField.FieldType) 
                select Expression.Bind(targetField, Expression.Field(parameter, sourceField)));
            return bindings;
        }
    }
}
