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
    public static class SetSuspendStateWrapper
    {
        [DllImport ("Powrprof.dll", SetLastError = true)]
        static extern bool SetSuspendState (
            bool hibernate, 
            bool forceCritical, 
            bool disableWakeEvent);

        /// <summary>
        /// Suspends the system by shutting power down. Depending on the Hibernate parameter, 
        /// the system either enters a suspend (sleep) state or hibernation (S4).
        /// </summary>
        /// <param name="hibernate">If this parameter is TRUE, the system hibernates. 
        /// If the parameter is FALSE, the system is suspended.</param>
        /// <param name="disableWakeEvent">If this parameter is TRUE, the system disables all wake events. 
        /// If the parameter is FALSE, any system wake events remain enabled.</param>
        /// <returns></returns>
        public static void SetSuspendState(bool hibernate,
            bool disableWakeEvent)
        {
            var returnValue = SetSuspendState(
                hibernate, 
                false, 
                disableWakeEvent);

            if (!returnValue)
            {
                throw new Win32Exception();
            }
        }

    }
}
