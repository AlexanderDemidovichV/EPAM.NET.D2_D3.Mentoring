using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace InterpolationToConcatenation.InterpolationReplacer
{
    public class InterpolationToConcatenationExpressionVisitor : ExpressionVisitor
    {
        private const string FormatMethodName = "Format";
        private const string ConcatMethodName = "Concat";

        private readonly List<PatternMachingStructure> _patternMatchingList;

        private IEnumerable<MethodInfo> FormatMethods =>
            typeof(string).GetMethods().Where(x => x.Name.Contains(FormatMethodName));

        private IEnumerable<MethodInfo> FormatMethodWithArrayParameter => FormatMethods.
            Where(methodInfo => methodInfo.GetParameters().
            Any(parameterInfo => parameterInfo.ParameterType == typeof(object[])));

        private IEnumerable<MethodInfo> FormatMethodsWithObjects =>
            FormatMethods.Except(FormatMethodWithArrayParameter);

        private readonly Regex _regexPattern = new Regex(@"\{\d\}");

        public InterpolationToConcatenationExpressionVisitor()
        {
            _patternMatchingList = new List<PatternMachingStructure>
            {
                new PatternMachingStructure
                {
                    FilterPredicate = methodInfo => 
                        FormatMethodsWithObjects.Contains(methodInfo),
                    SelectorArgumentsFunc = x => x.Arguments.Skip(1),
                    ReturnFunc = InterpolationToStringConcat
                },
                new PatternMachingStructure
                {
                    FilterPredicate = 
                        methodInfo => FormatMethodWithArrayParameter.Contains(methodInfo),
                    SelectorArgumentsFunc = methodCallExpression => 
                        ((NewArrayExpression)methodCallExpression.Arguments.Last()).Expressions,
                    ReturnFunc = InterpolationToStringConcat
                },
                new PatternMachingStructure
                {
                    FilterPredicate = 
                        method => FormatMethods.All(formatMethod => formatMethod != method),
                    SelectorArgumentsFunc = 
                        methodCallExpression => methodCallExpression.Arguments,
                    ReturnFunc = (node, _) => base.VisitMethodCall(node)
                }
            };
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
            var pattern = _patternMatchingList.First(x => x.FilterPredicate(node.Method));
            var arguments = pattern.SelectorArgumentsFunc(node);
            var expression = pattern.ReturnFunc(node, arguments);
            return expression;
        }
    }
}
