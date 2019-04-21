using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.ServiceModel.Channels;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Diagnostics;
using Message = Microsoft.Azure.ServiceBus.Message;

namespace HandlerService
{
    public class HandlerService
    {
        //private static TelemetryClient _telemetryClient;

        //private static IQueueClient _queueClient;

        //private readonly Guid _guid;

        //private const string ServiceBusConnectionString =
        //    "Endpoint=sb://dzemidovich-dev.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=41JPYUS11Yx8b71bXdyLpoGsfsWRvNrIrBjkYYeTRS4=";
        //private const string SqlServerDatabaseConnectionString =
        //    "Server=tcp:dzemidovich-sql-server-dev.database.windows.net,1433;Initial Catalog=epam-mentoring;Persist Security Info=False;User ID=Gendalf;Password=AzureMentoringEpam2018;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        //private const string QueueName = "images";

        //public HandlerService(string outDirectory)
        //{
        //    _guid = Guid.NewGuid();
        //}

        public void Start()
        {
            //_queueClient = new QueueClient(ServiceBusConnectionString, QueueName);
            //_telemetryClient = new TelemetryClient(new TelemetryConfiguration("3d240292-a095-4b96-b7f9-23cc49cd21f7"));
            //var o = Create(new HandlerModel
            //{
            //    Guid = _guid,
            //    Delay = 30,
            //    Status = 1
            //});

            //RegisterOnMessageHandlerAndReceiveMessages();
        }

        public void Stop()
        {
            //_telemetryClient.Flush();
            //Remove(_guid);
            //_queueClient.CloseAsync().GetAwaiter().GetResult();
        }

        //private void RegisterOnMessageHandlerAndReceiveMessages()
        //{
        //    // Configure the MessageHandler Options in terms of exception handling, number of concurrent messages to deliver etc.
        //    var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
        //    {
        //        // Maximum number of Concurrent calls to the callback `ProcessMessagesAsync`, set to 1 for simplicity.
        //        // Set it according to how many messages the application wants to process in parallel.
        //        MaxConcurrentCalls = 1,

        //        // Indicates whether MessagePump should automatically complete the messages after returning from User Callback.
        //        // False below indicates the Complete will be handled by the User Callback as in `ProcessMessagesAsync` below.
        //        AutoComplete = false
        //    };

        //    // Register the function that will process messages
        //    _queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        //}

        //private async Task ProcessMessagesAsync(Message message, CancellationToken token)
        //{
        //    var activity = message.ExtractActivity();

        //    // If you are using Microsoft.ApplicationInsights package version 2.6-beta or higher, you should call StartOperation<RequestTelemetry>(activity) instead
        //    using (var operation = _telemetryClient.StartOperation<RequestTelemetry>("Process", activity.RootId, activity.ParentId))
        //    {
        //        _telemetryClient.TrackTrace("Received message");
        //        try
        //        {
        //            // Process the message
        //            //Console.WriteLine($"Received message: SequenceNumber:{message.SystemProperties.SequenceNumber} ");

        //            var myUserProperties = message.UserProperties;

        //            //Console.WriteLine($"guid={myUserProperties["guid"]}");

        //            // Complete the message so that it is not received again.
        //            // This can be done only if the queueClient is created in ReceiveMode.PeekLock mode (which is default).
        //            await _queueClient.CompleteAsync(message.SystemProperties.LockToken);

        //            // Note: Use the cancellationToken passed as necessary to determine if the queueClient has already been closed.
        //            // If queueClient has already been Closed, you may chose to not call CompleteAsync() or AbandonAsync() etc. calls 
        //            // to avoid unnecessary exceptions.

        //            await Task.Delay(TimeSpan.FromSeconds(5));
        //        }
        //        catch (Exception ex)
        //        {
        //            _telemetryClient.TrackException(ex);
        //            operation.Telemetry.Success = false;
        //            throw;
        //        }
        //        finally
        //        {
        //            _telemetryClient.StopOperation(operation);
        //        }
        //        operation.Telemetry.Success = true;
        //        _telemetryClient.TrackTrace("Done");
        //    }
        //    _telemetryClient.Flush();
        //}

        //// Use this Handler to look at the exceptions received on the MessagePump
        //private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        //{
        //    //Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
        //    //var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
        //    //Console.WriteLine("Exception context for troubleshooting:");
        //    //Console.WriteLine($"- Endpoint: {context.Endpoint}");
        //    //Console.WriteLine($"- Entity Path: {context.EntityPath}");
        //    //Console.WriteLine($"- Executing Action: {context.Action}");
        //    return Task.CompletedTask;
        //}

        //private HandlerModel Find(Guid guid)
        //{
        //    using (IDbConnection db = new SqlConnection(SqlServerDatabaseConnectionString))
        //    {
        //        return db.Query<HandlerModel>("Select * From Handlers " +
        //                                      "WHERE Guid = @Guid", new { guid }).SingleOrDefault();
        //    }
        //}
        //private int Update(HandlerModel handler)
        //{
        //    using (IDbConnection db = new SqlConnection(SqlServerDatabaseConnectionString))
        //    {
        //        var sqlQuery = "UPDATE Handlers SET Delay = @Delay, " +
        //                       " Status = @Status " + "WHERE Guid = @Guid";
        //        var rowsAffected = db.Execute(sqlQuery, handler);
        //        return rowsAffected;
        //    }
        //}

        //private HandlerModel Create(HandlerModel handler)
        //{
        //    using (IDbConnection db = new SqlConnection(SqlServerDatabaseConnectionString))
        //    {
        //        var sqlQuery = "INSERT Handlers VALUES(@Guid, @Status, @Delay)";
        //        return db.Query<HandlerModel>(sqlQuery, handler).SingleOrDefault();
        //    }
        //}

        //private int Remove(Guid guid)
        //{
        //    using (IDbConnection db = new SqlConnection(SqlServerDatabaseConnectionString))
        //    {
        //        return db.Execute("DELETE FROM Handlers WHERE Guid = @Guid", new { guid });
        //    }
        //}
    }
}
