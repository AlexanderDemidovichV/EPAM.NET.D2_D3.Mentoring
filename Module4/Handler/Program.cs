using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Diagnostics;

namespace Handler
{
    class Program
    {
        private static TelemetryClient telemetryClient;

        private static IQueueClient queueClient;

        private const string ServiceBusConnectionString = 
            "Endpoint=sb://dzemidovich-dev.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=41JPYUS11Yx8b71bXdyLpoGsfsWRvNrIrBjkYYeTRS4=";
        private const string QueueName = "images";

        static async Task Main()
        {
            queueClient = new QueueClient(ServiceBusConnectionString, QueueName);
            telemetryClient = new TelemetryClient(new TelemetryConfiguration("7da04e81-63ca-4b0b-9492-a6b2caf0df53"));

            RegisterOnMessageHandlerAndReceiveMessages();

            Console.ReadLine();
        }

        static void RegisterOnMessageHandlerAndReceiveMessages()
        {
            // Configure the MessageHandler Options in terms of exception handling, number of concurrent messages to deliver etc.
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                // Maximum number of Concurrent calls to the callback `ProcessMessagesAsync`, set to 1 for simplicity.
                // Set it according to how many messages the application wants to process in parallel.
                MaxConcurrentCalls = 1,

                // Indicates whether MessagePump should automatically complete the messages after returning from User Callback.
                // False below indicates the Complete will be handled by the User Callback as in `ProcessMessagesAsync` below.
                AutoComplete = false
            };

            // Register the function that will process messages
            queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }

        static async Task ProcessMessagesAsync(Message mess                         age, CancellationToken token)
        {
            var activity = message.ExtractActivity();

            // If you are using Microsoft.ApplicationInsights package version 2.6-beta or higher, you should call StartOperation<RequestTelemetry>(activity) instead
            using (var operation = telemetryClient.StartOperation<RequestTelemetry>("Process", activity.RootId, activity.ParentId))
            {
                telemetryClient.TrackTrace("Received message");
                try
                {
                    // Process the message
                    Console.WriteLine($"Received message: SequenceNumber:{message.SystemProperties.SequenceNumber} ");//Body:{Encoding.UTF8.GetString(message.Body)}");

                    var myUserProperties = message.UserProperties;

                    Console.WriteLine($"guid={myUserProperties["guid"]}");

                    // Complete the message so that it is not received again.
                    // This can be done only if the queueClient is created in ReceiveMode.PeekLock mode (which is default).
                    await queueClient.CompleteAsync(message.SystemProperties.LockToken);

                    // Note: Use the cancellationToken passed as necessary to determine if the queueClient has already been closed.
                    // If queueClient has already been Closed, you may chose to not call CompleteAsync() or AbandonAsync() etc. calls 
                    // to avoid unnecessary exceptions.

                    await Task.Delay(TimeSpan.FromSeconds(40));
                }
                catch (Exception ex)
                {
                    telemetryClient.TrackException(ex);
                    operation.Telemetry.Success = false;
                    throw;
                }

                telemetryClient.TrackTrace("Done");
            }
            
        }

        // Use this Handler to look at the exceptions received on the MessagePump
        static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: {context.Endpoint}");
            Console.WriteLine($"- Entity Path: {context.EntityPath}");
            Console.WriteLine($"- Executing Action: {context.Action}");
            return Task.CompletedTask;
        }

    }
}
