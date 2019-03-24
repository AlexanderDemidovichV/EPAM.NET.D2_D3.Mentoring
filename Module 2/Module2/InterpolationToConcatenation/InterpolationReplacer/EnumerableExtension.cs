using System.Collections.Generic;
using System.Linq;

namespace InterpolationToConcatenation.InterpolationReplacer
{
    public static class EnumerableExtension
    {
        public static IEnumerable<T> Merge<T>(this IEnumerable<T> sequence1,
            IEnumerable<T> sequence2)
        {
            var s1 = sequence1 as T[] ?? sequence1.ToArray();
            var s2 = sequence2 as T[] ?? sequence2.ToArray();

            var remainder = s1.Length > s2.Length ? 
                s1.Skip(s2.Length) : 
                s2.Skip(s1.Length);

            var assignments = s1.Zip(s2, (x, y) => new { x, y});

            foreach (var assignment in assignments)
            {
                yield return assignment.x;
                yield return assignment.y;
            }
            
            foreach (var expression in remainder)
            {
                yield return expression;
            }
        }
    }
}
