using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Messages;
using Microsoft.Azure.ServiceBus;
using MonitoringServer.Models;
using Newtonsoft.Json;

namespace MonitoringServer.Infastructure
{
    public class SubscriptionClientHandlers
    {
        private ISubscriptionClient _subscriptionClient;

        private const string ServiceBusConnectionString =
            "Endpoint=sb://dzemidovich-dev.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=41JPYUS11Yx8b71bXdyLpoGsfsWRvNrIrBjkYYeTRS4=";

        private const string TopicName = "handlerstopic";

        private const string SubscriptionName = "monitorserver";

        private const int minimumDelayBetweenHandlersResponses = 60;

        private readonly IDictionary<string, HandlerViewModel> _handlerStatuses;

        public SubscriptionClientHandlers(IDictionary<string, HandlerViewModel> handlerStatuses)
        {
            _handlerStatuses = handlerStatuses;
            _subscriptionClient = new SubscriptionClient(ServiceBusConnectionString, TopicName, SubscriptionName);
            RegisterOnMessageHandlerAndReceiveMessages();
        }

        private void RegisterOnMessageHandlerAndReceiveMessages()
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };
            
            _subscriptionClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }

        private async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            var statusMessage = JsonConvert.DeserializeObject<UpdateHandlerStatusMessage>(Encoding.UTF8.GetString(message.Body));
            var guid = statusMessage.HandlerGuid.ToString();
            if (!_handlerStatuses.ContainsKey(guid))
            {
                _handlerStatuses.Add(guid, new HandlerViewModel{Guid = guid});
            }

            var handler = _handlerStatuses[guid];
            if (message.SystemProperties.EnqueuedTimeUtc > handler.LastUpdated)
            {
                switch (statusMessage.Status)
                {
                    case UpdateHandlerType.Register:
                        handler.Status = HandlerStatus.Running;
                        handler.LastUpdated = message.SystemProperties.EnqueuedTimeUtc;
                        break;
                    case UpdateHandlerType.Receive:
                        handler.Status = HandlerStatus.Running;
                        handler.LastUpdated = message.SystemProperties.EnqueuedTimeUtc;
                        break;
                    case UpdateHandlerType.CompletedMessage:
                        handler.Status = HandlerStatus.Idle;
                        handler.LastUpdated = message.SystemProperties.EnqueuedTimeUtc;
                        break;
                    case UpdateHandlerType.Unregister:
                        handler.Status = HandlerStatus.Running;
                        handler.LastUpdated = message.SystemProperties.EnqueuedTimeUtc;
                        break;
                }
            }

            await _subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            return Task.CompletedTask;
        }
    }
}
