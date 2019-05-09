using System;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Diagnostics;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Serialization;

namespace ServiceTelemetry.Aspects
{
    [PSerializable]
    public class RequiresServiceRequestTelemetry : RequiresServiceTelemetry
    {
        protected IOperationHolder<RequestTelemetry> Operation { get; set; }

        [ImportMember("_guid")]
        public Property<Guid> Guid;

        public override void OnEntry(MethodExecutionArgs args)
        {
            _telemetryClient = new TelemetryClient(
                new TelemetryConfiguration(InstrumentationKey));
            var message = (Message) args.Arguments[0];
            var activity = message.ExtractActivity();

            Operation = 
                _telemetryClient.StartOperation<RequestTelemetry>(
                    "Process", activity.RootId, activity.ParentId);

            _telemetryClient.TrackTrace("Received message");
            _telemetryClient.Context.Properties["HandlerId"] = Guid.Get().ToString();
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
