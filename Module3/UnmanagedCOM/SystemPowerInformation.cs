using System.Runtime.InteropServices;

namespace PowerManagementCOM
{
    [StructLayout(LayoutKind.Sequential)]
    [ComVisible(true)]
    public struct SystemPowerInformation
    {
        public uint MaxIdlenessAllowed;
        public uint Idleness;
        public uint TimeRemaining;
        public sbyte CoolingMode;
    }
}
