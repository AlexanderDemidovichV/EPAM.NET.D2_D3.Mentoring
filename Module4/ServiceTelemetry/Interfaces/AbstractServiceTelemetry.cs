using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using PostSharp.Aspects;
using PostSharp.Serialization;

namespace ServiceTelemetry.Interfaces
{
    [PSerializable]
    public abstract class AbstractServiceTelemetry : OnMethodBoundaryAspect, IInstanceScopedAspect
    {
        [PNonSerialized]
        protected TelemetryClient _telemetryClient;

        protected const string InstrumentationKey = "3d240292-a095-4b96-b7f9-23cc49cd21f7";
        
        public object CreateInstance(AdviceArgs adviceArgs)
        {
            return this.MemberwiseClone();
        }

        public void RuntimeInitializeInstance()
        {
        }
    }
}
