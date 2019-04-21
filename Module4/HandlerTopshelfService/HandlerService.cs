using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Diagnostics;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace HandlerTopshelfService
{
    public class HandlerService
    {
        private static TelemetryClient _telemetryClient;

        private static IQueueClient _queueClient;

        private readonly Guid _guid;

        private HandlerModel _handlerModel;

        private const string ServiceBusConnectionString =
            "Endpoint=sb://dzemidovich-dev.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=41JPYUS11Yx8b71bXdyLpoGsfsWRvNrIrBjkYYeTRS4=";
        private const string SqlServerDatabaseConnectionString =
            "Server=tcp:dzemidovich-sql-server-dev.database.windows.net,1433;Initial Catalog=epam-mentoring;Persist Security Info=False;User ID=Gendalf;Password=AzureMentoringEpam2018;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        private const string QueueName = "images";

        public HandlerService(string outDirectory)
        {
            _guid = Guid.NewGuid();
            if (!Directory.Exists("D:\\output"))
                Directory.CreateDirectory("D:\\output");
        }

        public void Start()
        {
            _queueClient = new QueueClient(ServiceBusConnectionString, QueueName);
            _telemetryClient = new TelemetryClient(new TelemetryConfiguration("3d240292-a095-4b96-b7f9-23cc49cd21f7"));
            _handlerModel = Create(new HandlerModel
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
            Remove(_handlerModel.Guid);
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
            
            using (var operation = _telemetryClient.StartOperation<RequestTelemetry>("Process", activity.RootId, activity.ParentId))
            {
                _telemetryClient.TrackTrace("Received message");
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(_handlerModel.Delay));
                    MutateEncodedImage(Encoding.UTF8.GetString(message.Body), $"D:\\output\\{Guid.NewGuid()}.jpg");
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
            //Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            //var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            //Console.WriteLine("Exception context for troubleshooting:");
            //Console.WriteLine($"- Endpoint: {context.Endpoint}");
            //Console.WriteLine($"- Entity Path: {context.EntityPath}");
            //Console.WriteLine($"- Executing Action: {context.Action}");
            return Task.CompletedTask;
        }

        private void MutateEncodedImage(string encoded, string outputPath)
        {
            using (var image = Image.Load(Base64ToByteArray(encoded)))
            {
                MutateImage(image);
                image.Save(outputPath);
            }
        }

        private byte[] Base64ToByteArray(string data)
        {
            return Convert.FromBase64String(data);
        }

        private Image<Rgba32> MutateImage(Image<Rgba32> image)
        {
            double width, height;
            if (image.Width > image.Height)
            {
                width = 200;
                height = image.Height * (width / image.Width);
            }
            else
            {
                height = 200;
                width = image.Width * (height / image.Height);
            }

            image.Mutate(x => x
                .Resize((int)width, (int)height)
                .Grayscale());
            return image;
        }

        private HandlerModel Find(Guid guid)
        {
            using (IDbConnection db = new SqlConnection(SqlServerDatabaseConnectionString))
            {
                return db.Query<HandlerModel>("Select * From Handlers " +
                                              "WHERE Guid = @Guid", new { guid }).SingleOrDefault();
            }
        }
        private int Update(HandlerModel handler)
        {
            using (IDbConnection db = new SqlConnection(SqlServerDatabaseConnectionString))
            {
                var sqlQuery = "UPDATE Handlers SET Delay = @Delay, " +
                               " Status = @Status " + "WHERE Guid = @Guid";
                var rowsAffected = db.Execute(sqlQuery, handler);
                return rowsAffected;
            }
        }

        private HandlerModel Create(HandlerModel handler)
        {
            using (IDbConnection db = new SqlConnection(SqlServerDatabaseConnectionString))
            {
                var sqlQuery = "INSERT Handlers VALUES(@Guid, @Status, @Delay)";
                return db.Query<HandlerModel>(sqlQuery, handler).SingleOrDefault();
            }
        }

        private int Remove(Guid guid)
        {
            using (IDbConnection db = new SqlConnection(SqlServerDatabaseConnectionString))
            {
                return db.Execute("DELETE FROM Handlers WHERE Guid = @Guid", new { guid });
            }
        }

        private void UpdateHandlerEntity()
        {
            var entity = Find(_guid);
            _handlerModel = entity;
        }
    }
}
