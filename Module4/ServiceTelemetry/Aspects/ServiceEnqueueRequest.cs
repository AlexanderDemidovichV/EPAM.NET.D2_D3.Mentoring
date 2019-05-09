using System.Diagnostics;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Serialization;
using ServiceTelemetry.Interfaces;

namespace ServiceTelemetry.Aspects
{
    [PSerializable]
    public class ServiceEnqueueRequest : RequiresServiceTelemetry
    {
        [ImportMember("QueueName")]
        public Property<string> QueueName;

        public override void OnEntry(MethodExecutionArgs args)
        {
            _telemetryClient = new TelemetryClient(
                new TelemetryConfiguration(InstrumentationKey));
            var activity = new Activity("Queue");

            Operation = _telemetryClient.StartOperation<DependencyTelemetry>(activity);
            Operation.Telemetry.Type = "Queue";
            Operation.Telemetry.Data = "Enqueue " + QueueName;
        }
    }
}
