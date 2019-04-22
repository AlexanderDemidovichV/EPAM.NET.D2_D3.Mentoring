using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Diagnostics;

namespace HandlerTopshelfService
{
    public class HandlerService
    {
        private static TelemetryClient _telemetryClient;

        private static IQueueClient _queueClient;

        private readonly Guid _guid;

        private HandlerModel _handlerModel;

        private readonly MutateImageHelper _mutateImageHelper;

        private const string ServiceBusConnectionString =
            "Endpoint=sb://dzemidovich-dev.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=41JPYUS11Yx8b71bXdyLpoGsfsWRvNrIrBjkYYeTRS4=";
        private const string QueueName = "images";

        private readonly string _outDirectory;

        public HandlerService(string outDirectory)
        {
            _outDirectory = outDirectory;
            _guid = Guid.NewGuid();
            _mutateImageHelper = new MutateImageHelper();
            if (!Directory.Exists(outDirectory))
                Directory.CreateDirectory(outDirectory);
        }

        public void Start()
        {
            _queueClient = new QueueClient(ServiceBusConnectionString, QueueName);
            _telemetryClient = new TelemetryClient(
                new TelemetryConfiguration("3d240292-a095-4b96-b7f9-23cc49cd21f7"));
            _handlerModel = HandlerHelper.Create(new HandlerModel
            {
                Guid = _guid,
                Delay = 15,
                Status = 1
            });
            
            RegisterOnMessageHandlerAndReceiveMessages();
        }

        public void Stop()
        {
            _telemetryClient.Flush();
            HandlerHelper.Remove(_handlerModel.Guid);
            _queueClient.CloseAsync().GetAwaiter().GetResult();
        }

        private void RegisterOnMessageHandlerAndReceiveMessages()
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };
            
            _queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }

        private async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            UpdateHandlerEntity();
            var activity = message.ExtractActivity();
            
            using (var operation = _telemetryClient.
                StartOperation<RequestTelemetry>("Process", activity.RootId, activity.ParentId))
            {
                _telemetryClient.TrackTrace("Received message");
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(_handlerModel.Delay)); 
                    var myUserProperties = message.UserProperties;
                    Console.WriteLine($"guid={myUserProperties["guid"]}");
                    _mutateImageHelper.MutateEncodedImage(
                        Encoding.UTF8.GetString(message.Body), 
                        $"{_outDirectory}\\{myUserProperties["guid"]}_{DateTime.Now:h_mm_ss_fff}.jpg");

                    _telemetryClient.Context.Properties["HandlerId"] = _handlerModel.Guid.ToString();
                    await _queueClient.CompleteAsync(message.SystemProperties.LockToken);
                }
                catch (Exception ex)
                {
                    _telemetryClient.TrackException(ex);
                    operation.Telemetry.Success = false;
                    throw;
                }
                finally
                {
                    _telemetryClient.StopOperation(operation);
                }
                operation.Telemetry.Success = true;
                _telemetryClient.TrackTrace("Done");
            }
            _telemetryClient.Flush();
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            return Task.CompletedTask;
        }

        private void UpdateHandlerEntity()
        {
            var entity = HandlerHelper.Find(_guid);
            _handlerModel = entity;
        }
    }
}
