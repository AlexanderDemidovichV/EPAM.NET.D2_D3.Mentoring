using System;

namespace Unmanaged.Infastructure
{
    /// <summary>
    /// Indicates the specific power information to be set or retrieved. It used as a parameter in CallNtPowerInformation function.
    /// </summary>
    public enum PowerInformationLevel : Int32
    {
        /// <summary>
        /// <para> The lpInBuffer parameter must be NULL; otherwise, the function returns ERROR_INVALID_PARAMETER.</para>
        ///The lpOutputBuffer buffer receives a ULONGLONG that specifies the interrupt-time count, in 100-nanosecond units, at the last system sleep time.
        /// </summary>
        LastSleepTime = 15,
        /// <summary>
        /// <para> The lpInBuffer parameter must be NULL; otherwise, the function returns ERROR_INVALID_PARAMETER.</para>
        /// The lpOutputBuffer buffer receives a ULONGLONG that specifies the interrupt-time count, in 100-nanosecond units, at the last system wake time.
        /// </summary>
        LastWakeTime = 14,
        /// <summary>
        /// <para> The lpInBuffer parameter must be NULL; otherwise, the function returns ERROR_INVALID_PARAMETER.</para>
        /// The lpOutputBuffer buffer receives a SYSTEM_BATTERY_STATE structure containing information about the current system battery.
        /// </summary>
        SystemBatteryState = 5,
        /// <summary>
        /// <para> The lpInBuffer parameter must be NULL; otherwise, the function returns ERROR_INVALID_PARAMETER.</para>
        /// The lpOutputBuffer buffer receives a SYSTEM_POWER_INFORMATION structure.
        /// <para> Applications can use this level to retrieve information about the idleness of the system.</para>
        /// </summary>
        SystemPowerInformation = 12,
        /// <summary>
        /// <para> If lpInBuffer is not NULL and the current user has sufficient privileges, the function commits or decommits the storage required to hold the hibernation image on the boot volume.</para>
        /// The lpInBuffer parameter must point to a BOOLEAN value indicating the desired request. If the value is TRUE, the hibernation file is reserved; if the value is FALSE, the hibernation file is removed.
        /// </summary>
        SystemReserveHiberFile = 10
    }
}
