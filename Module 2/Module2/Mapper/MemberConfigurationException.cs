using System;

namespace Mapper
{
    public class MemberConfigurationException : Exception
    {
        public MemberConfigurationException()
        {
        }

        public MemberConfigurationException(string message)
            : base(message)
        {
        }

        public MemberConfigurationException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
