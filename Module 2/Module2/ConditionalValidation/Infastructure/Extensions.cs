using System;
using System.Collections.Generic;
using System.Text;

namespace ConditionalValidation.Infastructure
{
    public static class Extensions
    {
        internal static void Guard(this object obj, string message, string paramName)
        {
            if (obj == null)
                throw new ArgumentNullException(paramName, message);
        }
    }
}
