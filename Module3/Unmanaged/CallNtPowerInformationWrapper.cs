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
            int nInputBufferSize,
            IntPtr lpOutputBuffer,
            int nOutputBufferSize);

        public static TOutput GetNtPowerInformationValue<TOutput>(
            PowerInformationLevel informationLevel)
        {
            IntPtr outputBuffer = 
                Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(TOutput)));

            var returnValue = CallNtPowerInformation(
                (int)informationLevel, 
                IntPtr.Zero, 
                0, 
                outputBuffer,
                Marshal.SizeOf(typeof(TOutput)));

            if (returnValue != (uint)NtStatus.Success)
            {
                throw new Win32Exception(
                    $"{(NtStatus)Enum.Parse(typeof(NtStatus), returnValue.ToString())}");
            }

            return Marshal.PtrToStructure<TOutput>(outputBuffer);
        }

        public static void GetNtPowerInformationValue<TInput>
            (PowerInformationLevel informationLevel, IntPtr inPtr)
        {
            var returnValue = CallNtPowerInformation(
                (int)informationLevel,
                inPtr,
                Marshal.SizeOf(typeof(TInput)),
                IntPtr.Zero,
                0
            );
            if (returnValue != (uint)NtStatus.Success)
            {
                throw new Win32Exception(
                    $"{(NtStatus)Enum.Parse(typeof(NtStatus), returnValue.ToString())}");
            }
        }
    }

    
}
