using System.Linq;

namespace InterpolationToConcatenation.InterpolationReplacer
{
    public static class QueryableExtension
    {
        public static IQueryable<T> InterpolationToConcatenation<T>(this IQueryable<T> qu)
        {
            var expression = new InterpolationToConcatenationExpressionVisitor().
                Visit(qu.Expression);
            return qu.Provider.CreateQuery<T>(expression);
        }
    }
}
