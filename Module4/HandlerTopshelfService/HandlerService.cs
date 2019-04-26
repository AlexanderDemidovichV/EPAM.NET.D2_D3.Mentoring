using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Messages;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Diagnostics;
using Newtonsoft.Json;

namespace HandlerTopshelfService
{
    public class HandlerService
    {
        private const string TopicName = "handlerstopic";

        private static TelemetryClient _telemetryClient;

        private static ITopicClient _topicClient;

        private static IQueueClient _queueClient;

        private readonly Guid _guid;

        private HandlerModel _handlerModel;

        private readonly MutateImageHelper _mutateImageHelper;

        private const string ServiceBusConnectionString =
            "Endpoint=sb://dzemidovich-dev.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=41JPYUS11Yx8b71bXdyLpoGsfsWRvNrIrBjkYYeTRS4=";
        private const string QueueName = "images";

        private readonly string _outDirectory;

        private const string InstrumentationKey = "3d240292-a095-4b96-b7f9-23cc49cd21f7";

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
            _topicClient = new TopicClient(ServiceBusConnectionString, TopicName);
            _queueClient = new QueueClient(ServiceBusConnectionString, QueueName);
            _telemetryClient = new TelemetryClient(
                new TelemetryConfiguration(InstrumentationKey));
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
            SendMessageToTopic(new UpdateHandlerStatusMessage
            {
                HandlerGuid = _guid,
                Status = UpdateHandlerType.Unregister
            }).GetAwaiter().GetResult();
            _telemetryClient.Flush();
            HandlerHelper.Remove(_guid);
            _queueClient.CloseAsync().GetAwaiter().GetResult();
            _topicClient.CloseAsync().GetAwaiter().GetResult();
        }

        private void RegisterOnMessageHandlerAndReceiveMessages()
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };
            
            _queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
            SendMessageToTopic(new UpdateHandlerStatusMessage
            {
                HandlerGuid = _guid,
                Status = UpdateHandlerType.Register
            }).GetAwaiter().GetResult();
        }

        private async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            UpdateHandlerEntity();
            var activity = message.ExtractActivity();
            
            await SendMessageToTopic(new UpdateHandlerStatusMessage
            {
                HandlerGuid = _guid,
                Status = UpdateHandlerType.Receive
            });

            using (var operation = _telemetryClient.
                StartOperation<RequestTelemetry>("Process", activity.RootId, activity.ParentId))
            {
                _telemetryClient.TrackTrace("Received message");
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(_handlerModel.Delay)); 
                    var myUserProperties = message.UserProperties;
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

            await SendMessageToTopic(new UpdateHandlerStatusMessage
            {
                HandlerGuid = _guid,
                Status = UpdateHandlerType.CompletedMessage
            });
        }

        private async Task SendMessageToTopic(UpdateHandlerStatusMessage messageData)
        {
            var activity = new Activity("Handler Status");
            using (var operation = _telemetryClient.StartOperation<DependencyTelemetry>(activity))
            {
                operation.Telemetry.Type = "Handler Status";

                try
                {
                    var messageBody = JsonConvert.SerializeObject(messageData);
                    var message = new Message(Encoding.UTF8.GetBytes(messageBody));
                    message.UserProperties.Add("guid", _guid.ToString());

                    await _topicClient.SendAsync(message);

                    operation.Telemetry.Success = true;
                }
                catch (Exception exception)
                {
                    _telemetryClient.TrackException(exception);
                    operation.Telemetry.Success = false;
                }
                finally
                {
                    _telemetryClient.StopOperation(operation);
                    _telemetryClient.Flush();
                }
            }
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
