using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PostSharp.Aspects;
using PostSharp.Serialization;
using System.Diagnostics;
using Messages;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Diagnostics;
using PostSharp.Aspects.Advices;

namespace HandlerTopshelfService
{
    [PSerializable]
    public class RequiresServiceRequestTelemetryAttribute2 : OnMethodBoundaryAspect, IInstanceScopedAspect
    {
        [ImportMember("_guid")]
        public Property<Guid> Guid;

        private IOperationHolder<RequestTelemetry> _operation;

        [PNonSerialized]
        private TelemetryClient _telemetryClient;

        private string[] _parameterNames;

        private const string InstrumentationKey = "3d240292-a095-4b96-b7f9-23cc49cd21f7";

        public override void CompileTimeInitialize(MethodBase method, AspectInfo aspectInfo)
        {
            this._parameterNames = method.GetParameters().Select(p => p.Name).ToArray();
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
            _telemetryClient = new TelemetryClient(
                new TelemetryConfiguration(InstrumentationKey));
            var message = (Message) args.Arguments[0];
            var activity = message.ExtractActivity();

            _operation = 
                _telemetryClient.StartOperation<RequestTelemetry>(
                    "Process", activity.RootId, activity.ParentId);

            _telemetryClient.TrackTrace("Received message");
            _telemetryClient.Context.Properties["HandlerId"] = Guid.Get().ToString();
            
            var arguments = new List<KeyValuePair<string, object>>();
            for (int i = 0; i < this._parameterNames.Length; i++)
            {
                arguments.Add(new KeyValuePair<string, object>(this._parameterNames[i], args.Arguments[i]));
            }
        }
        // var transactionScope = new TransactionScope(TransactionScopeOption.Required);
        //args.MethodExecutionTag = transactionScope;

        public object CreateInstance(AdviceArgs adviceArgs)
        {
            return this.MemberwiseClone();
        }

        public void RuntimeInitializeInstance()
        {
        }
    
        public override void OnSuccess(MethodExecutionArgs args)
        {
            _operation.Telemetry.Success = true;
            //var transactionScope = (TransactionScope)args.MethodExecutionTag;
            // transactionScope.Complete();
        }

        public override void OnException(MethodExecutionArgs args)
        {
            _telemetryClient.TrackException(args.Exception);
            _operation.Telemetry.Success = false;
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            _telemetryClient.StopOperation(_operation);
            _telemetryClient.Flush();
            _operation.Dispose();
            //var transactionScope = (TransactionScope)args.MethodExecutionTag;
            //transactionScope.Dispose();
        }
    }
}
