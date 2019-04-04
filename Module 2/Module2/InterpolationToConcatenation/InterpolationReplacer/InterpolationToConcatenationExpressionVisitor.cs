using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace InterpolationToConcatenation.InterpolationReplacer
{
    public class InterpolationToConcatenationExpressionVisitor : ExpressionVisitor
    {
        private const string FormatMethodName = "FormatMethodName";
        private const string ConcatMethodName = "ConcatMethodName";

        private readonly IEnumerable<MethodInfo> _formatMethods;
        private readonly IEnumerable<MethodInfo> _formatMethodWithArrayParameter;
        
        private readonly Regex _regexPattern = new Regex(@"\{\d\}");

        public InterpolationToConcatenationExpressionVisitor()
        {
            _formatMethods = typeof(string).GetMethods().
                Where(x => x.Name.Contains(FormatMethodName));

            _formatMethodWithArrayParameter = _formatMethods.Where(methodInfo =>
                methodInfo.GetParameters().
                Any(parameterInfo => parameterInfo.ParameterType == typeof(object[])));
        }

        private Expression InterpolationToStringConcat(MethodCallExpression node,
            IEnumerable<Expression> formatArguments)
        {
            var formatString = node.Arguments.First();
            var argumentStrings = _regexPattern.Matches(formatString.ToString()).
                Select(match => Expression.Constant(match.Value));
            var merge = argumentStrings.Merge(formatArguments);
            var stringConcatMethod = 
                typeof(string).GetMethod(ConcatMethodName, new[] { typeof(object), typeof(object) });
            var result = merge.Aggregate((acc, cur) => Expression.Add(acc, cur, stringConcatMethod));
            return result;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (IsFormatMethodWithObjectsParams(node.Method))
            {
                var arguments = node.Arguments.Skip(1);
                var expression = InterpolationToStringConcat(node, arguments);
                return expression;
            }
            if (IsFormatMethodWithArrayParam(node.Method))
            {
                var arguments = ((NewArrayExpression)node.Arguments.Last()).Expressions;
                var expression = InterpolationToStringConcat(node, arguments);
                return expression;
            }
            if (IsNotFormatMethod(node.Method))
            {
                return base.VisitMethodCall(node);
            }
            throw new InterpolationToConcatenationException();
        }

        private bool IsFormatMethodWithObjectsParams(MethodInfo method)
        {
            var formatMethodsWithObjects =
                _formatMethods.Except(_formatMethodWithArrayParameter);
            return formatMethodsWithObjects.Contains(method);
        }

        private bool IsFormatMethodWithArrayParam(MethodInfo method)
        {
            return _formatMethodWithArrayParameter.Contains(method);
        }

        private bool IsNotFormatMethod(MethodInfo method)
        {
            return _formatMethods.All(formatMethod => formatMethod != method);
        }
    }
}
