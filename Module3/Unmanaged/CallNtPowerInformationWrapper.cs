using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Unmanaged.Infastructure;

namespace Unmanaged
{
    public static class CallNtPowerInformationWrapper
    {
        [DllImport("powrprof.dll", SetLastError = true)]
        private static extern uint CallNtPowerInformation(
            int informationLevel,
            IntPtr lpInputBuffer,
            uint nInputBufferSize,
            IntPtr lpOutputBuffer,
            uint nOutputBufferSize);

        public static TOutput GetNtPowerInformationLastSleepTime<TOutput>()
        {
            IntPtr outputBuffer = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(TOutput)));

            var returnValue = CallNtPowerInformation(
                (int)PowerInformationLevel.LastSleepTime, IntPtr.Zero, 0, 
                outputBuffer, (uint)Marshal.SizeOf(typeof(TOutput)) * 2);

            if (returnValue != (uint)NtStatus.Success)
            {
                throw new Win32Exception(
                    $"{(NtStatus)Enum.Parse(typeof(NtStatus), returnValue.ToString())}");
            }

            return Marshal.PtrToStructure<TOutput>(outputBuffer);
        }
    }

    
}
