namespace Unmanaged.Infastructure
{
    public struct SystemPowerInformation
    {
        public ulong MaxIdlenessAllowed;
        public ulong Idleness;
        public ulong TimeRemaining;
        public char CoolingMode;
    }
}
