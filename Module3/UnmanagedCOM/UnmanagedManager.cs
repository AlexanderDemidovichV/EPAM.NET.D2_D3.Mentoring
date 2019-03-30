using System;
using System.Runtime.InteropServices;
using System.Text;
using Unmanaged;
using Unmanaged.Infastructure;

namespace UnmanagedCOM
{
    [ComVisible(true)]
    [Guid("5FCF9787-E2BC-4862-9FAE-C109C39ADE40")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IUnmanagedManager
    {
        void Hibernate();
        void ReserveHibernationFile();
        void RemoveHibernationFile();
        string GetPowerInfo();
    }

    [ComVisible(true)]
    [Guid("9C857C6F-739B-4CA1-9722-0E4AD1140F97")]
    [ClassInterface(ClassInterfaceType.None)]
    public class UnmanagedManager : IUnmanagedManager
    {
        public void Hibernate()
        {
            SetSuspendStateWrapper.SetSuspendState(true, false);
        }

        public void ReserveHibernationFile()
        {
            var input = Marshal.AllocCoTaskMem(Marshal.SizeOf<bool>());
            Marshal.WriteByte(input, 1);
            CallNtPowerInformationWrapper.
                GetNtPowerInformationValue<bool>(PowerInformationLevel
                    .SystemReserveHiberFile, input);
        }

        public void RemoveHibernationFile()
        {
            var input = Marshal.AllocCoTaskMem(Marshal.SizeOf<bool>());
            Marshal.WriteByte(input, 0);
            CallNtPowerInformationWrapper.
                GetNtPowerInformationValue<bool>(PowerInformationLevel
                    .SystemReserveHiberFile, input);
        }

        public string GetPowerInfo()
        {
            var result = new StringBuilder();
            var spi = CallNtPowerInformationWrapper.
                GetNtPowerInformationValue<SystemPowerInformation>(
                PowerInformationLevel.SystemPowerInformation);
            var sbs = CallNtPowerInformationWrapper.
                GetNtPowerInformationValue<SystemBatteryState>(
                PowerInformationLevel.SystemBatteryState);

            result.AppendLine($@"Last Sleep Time = {CallNtPowerInformationWrapper.
                GetNtPowerInformationValue<ulong>(PowerInformationLevel.LastSleepTime)}");
            result.AppendLine($@"Last Wake Time = {CallNtPowerInformationWrapper.
                GetNtPowerInformationValue<ulong>(PowerInformationLevel.LastWakeTime)}");
            result.AppendLine("System battery state is:");
            result.AppendLine($@" AcOnLine = {sbs.AcOnLine} {(Char)10} 
                BatteryPresent = {sbs.BatteryPresent} {(Char)10} 
                Charging = {sbs.Charging} {(Char)10} 
                Discharging = {sbs.Discharging} {(Char)10} 
                MaxCapacity = {sbs.MaxCapacity} {(Char)10} 
                RemainingCapacity = {sbs.RemainingCapacity} {(Char)10} 
                Rate = {sbs.Rate} {(Char)10} 
                EstimatedTime = {sbs.EstimatedTime} {(Char)10} 
                DefaultAlert1 = {sbs.DefaultAlert1} {(Char)10} 
                DefaultAlert2 = {sbs.DefaultAlert2}");
            result.AppendLine("System power information is:");
            result.AppendLine($@" MaxIdlenessAllowed = {spi.MaxIdlenessAllowed} {(Char)10} 
                Idleness = {spi.Idleness} {(Char)10} 
                TimeRemaining = {spi.TimeRemaining} {(Char)10} 
                CoolingMode = {spi.CoolingMode}");
            return result.ToString();
        }
    }
}
