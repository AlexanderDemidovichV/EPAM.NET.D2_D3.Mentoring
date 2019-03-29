using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Unmanaged.Tests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void Test1()
        {
            var t = CallNtPowerInformationWrapper.GetNtPowerInformationLastSleepTime<int>();
        }
    }
}
