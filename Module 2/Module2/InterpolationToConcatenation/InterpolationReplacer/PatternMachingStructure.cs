using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace InterpolationToConcatenation.InterpolationReplacer
{
    public class PatternMachingStructure
    {
        public Func<MethodInfo, bool> FilterPredicate { get; set; }

        public Func<MethodCallExpression, IEnumerable<Expression>> SelectorArgumentsFunc { get; set; }

        public Func<MethodCallExpression, IEnumerable<Expression>, Expression> ReturnFunc { get; set; }
    }
}
