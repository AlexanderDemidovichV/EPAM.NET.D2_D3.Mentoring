using System;

namespace Mapper
{
    public class MemberConfigurationExpression : Exception
    {
        public MemberConfigurationExpression()
        {
        }

        public MemberConfigurationExpression(string message)
            : base(message)
        {
        }

        public MemberConfigurationExpression(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
