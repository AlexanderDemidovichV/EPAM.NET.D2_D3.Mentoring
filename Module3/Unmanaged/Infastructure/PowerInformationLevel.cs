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
        /// <para> The lpInBuffer parameter must be NULL; otherwise the function returns ERROR_INVALID_PARAMETER.</para>
        /// The lpOutputBuffer buffer receives one PROCESSOR_POWER_INFORMATION structure for each processor that is installed on the system. Use the GetSystemInfo function to retrieve the number of processors.
        /// </summary>
        ProcessorInformation = 11,
        /// <summary>
        /// <para> The lpInBuffer parameter must be NULL; otherwise, the function returns ERROR_INVALID_PARAMETER.</para>
        /// The lpOutputBuffer buffer receives a SYSTEM_BATTERY_STATE structure containing information about the current system battery.
        /// </summary>
        SystemBatteryState = 5,
        /// <summary>
        /// <para> The lpInBuffer parameter must be NULL; otherwise the function returns ERROR_INVALID_PARAMETER.</para>
        /// The lpOutputBuffer buffer receives a ULONG value containing the system execution state buffer.
        /// <para> This value may contain any combination of the following values: ES_SYSTEM_REQUIRED, ES_DISPLAY_REQUIRED, or ES_USER_PRESENT.</para>
        /// For more information, see the SetThreadExecutionState function.
        /// </summary>
        SystemExecutionState = 16,
        /// <summary>
        /// <para> The lpInBuffer parameter must be NULL, otherwise, the function returns ERROR_INVALID_PARAMETER.</para>
        /// The lpOutputBuffer buffer receives a SYSTEM_POWER_CAPABILITIES structure containing the current system power capabilities.
        /// <para> This information represents the currently supported power capabilities. It may change as drivers are installed in the system.
        /// For example, installation of legacy device drivers that do not support power management disables all system sleep states.</para>
        /// </summary>
        SystemPowerCapabilities = 4,
        /// <summary>
        /// <para> The lpInBuffer parameter must be NULL; otherwise, the function returns ERROR_INVALID_PARAMETER.</para>
        /// The lpOutputBuffer buffer receives a SYSTEM_POWER_INFORMATION structure.
        /// <para> Applications can use this level to retrieve information about the idleness of the system.</para>
        /// </summary>
        SystemPowerInformation = 12,
        /// <summary>
        /// <para> If lpInBuffer is not NULL, the function applies the SYSTEM_POWER_POLICY values passed in lpInBuffer to the current system power policy used while the system is running on AC (utility) power.</para>
        /// The lpOutputBuffer buffer receives a SYSTEM_POWER_POLICY structure containing the current system power policy used while the system is running on AC (utility) power.
        /// </summary>
        SystemPowerPolicyAc = 0,
        /// <summary>
        /// <para> The lpInBuffer parameter must be NULL; otherwise, the function returns ERROR_INVALID_PARAMETER.</para>
        /// The lpOutputBuffer buffer receives a SYSTEM_POWER_POLICY structure containing the current system power policy used while the system is running on AC (utility) power.
        /// </summary>
        SystemPowerPolicyCurrent = 8,
        /// <summary>
        /// <para> If lpInBuffer is not NULL, the function applies the SYSTEM_POWER_POLICY values passed in lpInBuffer to the current system power policy used while the system is running on battery power.</para>
        /// The lpOutputBuffer buffer receives a SYSTEM_POWER_POLICY structure containing the current system power policy used while the system is running on battery power.
        /// </summary>
        SystemPowerPolicyDc = 1,
        /// <summary>
        /// <para> If lpInBuffer is not NULL and the current user has sufficient privileges, the function commits or decommits the storage required to hold the hibernation image on the boot volume.</para>
        /// The lpInBuffer parameter must point to a BOOLEAN value indicating the desired request. If the value is TRUE, the hibernation file is reserved; if the value is FALSE, the hibernation file is removed.
        /// </summary>
        SystemReserveHiberFile = 10
    }
}
