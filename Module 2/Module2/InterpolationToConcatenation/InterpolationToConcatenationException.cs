using System;

namespace InterpolationToConcatenation
{
    public class InterpolationToConcatenationException: Exception
    {
        public InterpolationToConcatenationException()
        {
        }

        public InterpolationToConcatenationException(string message)
            : base(message)
        {
        }

        public InterpolationToConcatenationException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
