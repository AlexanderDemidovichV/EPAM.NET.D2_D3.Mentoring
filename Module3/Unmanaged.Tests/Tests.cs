using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Unmanaged.Infastructure;

namespace Unmanaged.Tests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void GetNtPowerInformation_GetLastSleepTime_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => CallNtPowerInformationWrapper.
                GetNtPowerInformationValue<long>(
                PowerInformationLevel.LastSleepTime));
        }

        [Test]
        public void GetNtPowerInformation_GetLastWakeTime_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => CallNtPowerInformationWrapper.
                GetNtPowerInformationValue<long>(
                    PowerInformationLevel.LastWakeTime));
        }

        [Test]
        public void GetNtPowerInformation_GetSystemBatteryState_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => CallNtPowerInformationWrapper.
                GetNtPowerInformationValue<SystemBatteryState>(
                    PowerInformationLevel.SystemBatteryState));
        }

        [Test]
        public void GetNtPowerInformation_GetSystemPowerInformation_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => CallNtPowerInformationWrapper.
                GetNtPowerInformationValue<SystemPowerInformation>(
                    PowerInformationLevel.SystemPowerInformation));
        }

        [Test]
        public void GetNtPowerInformation_GetSystemReserveHiberFile_DoesNotThrow()
        {
            IntPtr outputBuffer =
                Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(bool)));

            Marshal.WriteByte(outputBuffer, 1);
            //Trying to reserve hibernation file...
            Assert.DoesNotThrow(() => CallNtPowerInformationWrapper.
                GetNtPowerInformationValue<bool>(PowerInformationLevel
                    .SystemReserveHiberFile, outputBuffer));

            outputBuffer =
                Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(bool)));

            Marshal.WriteByte(outputBuffer, 0);
            //Trying to remove hibernation file...
            Assert.DoesNotThrow(() => CallNtPowerInformationWrapper.
                GetNtPowerInformationValue<bool>(PowerInformationLevel
                    .SystemReserveHiberFile, outputBuffer));
        }

        [Test]
        public void SetSuspendState_SetHibernatesState_SystemHibernates()
        {
            SetSuspendStateWrapper.SetSuspendState(true, false);
        }
    }
}
