using System.Diagnostics;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using PostSharp.Aspects;
using PostSharp.Serialization;
using ServiceTelemetry.Interfaces;

namespace ServiceTelemetry.Aspects
{
    [PSerializable]
    public class RequiresServiceTelemetry : AbstractServiceTelemetry
    {
        protected IOperationHolder<DependencyTelemetry> Operation { get; set; }

        public override void OnEntry(MethodExecutionArgs args)
        {
            _telemetryClient = new TelemetryClient(
                new TelemetryConfiguration(InstrumentationKey));
            var activity = new Activity("Handler Status. Dependency Telemetry Aspect");
            Operation = _telemetryClient.StartOperation<DependencyTelemetry>(activity);
            
            Operation.Telemetry.Type = "Handler Status. Dependency Telemetry Aspect";
        }

        public override void OnSuccess(MethodExecutionArgs args)
        {
            Operation.Telemetry.Success = true;
        }

        public override void OnException(MethodExecutionArgs args)
        {
            Operation.Telemetry.Success = false;
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            _telemetryClient.StopOperation(Operation);
            _telemetryClient.Flush();
            Operation.Dispose();
        }
    }
}
