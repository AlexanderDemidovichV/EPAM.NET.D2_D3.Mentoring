using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
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

namespace GeneratorTopshelfService
{
    public class GeneratorService
    {
        private static TelemetryClient telemetryClient =
            new TelemetryClient(new TelemetryConfiguration("7da04e81-63ca-4b0b-9492-a6b2caf0df53"));

        private IQueueClient queueClient;

        private const string ServiceBusConnectionString =
            "Endpoint=sb://dzemidovich-dev.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=41JPYUS11Yx8b71bXdyLpoGsfsWRvNrIrBjkYYeTRS4=";

        private const string SqlServerDatabaseConnectionString =
            "Server=tcp:dzemidovich-sql-server-dev.database.windows.net,1433;Initial Catalog=epam-mentoring;Persist Security Info=False;User ID=Gendalf;Password=AzureMentoringEpam2018;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        private const string QueueName = "images";

        private readonly Guid _guid;

        private GeneratorModel _generatorModel;

        private readonly CancellationTokenSource _cancellationTokenSource;

        private Task processTask;

        public GeneratorService(string outDirectory)
        {
            _guid = Guid.NewGuid();
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public void Start()
        {
            queueClient = new QueueClient(ServiceBusConnectionString, QueueName);
            
            telemetryClient = new TelemetryClient(new TelemetryConfiguration("3d240292-a095-4b96-b7f9-23cc49cd21f7"));
            _generatorModel = Create(new GeneratorModel()
            {
                Guid = _guid,
                Delay = 5
            });

            processTask = StartProcess(_cancellationTokenSource.Token);
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
            processTask.Wait();
            telemetryClient.Flush();
            Remove(_generatorModel.Guid);
            queueClient.CloseAsync().GetAwaiter().GetResult();
        }

        private async Task StartProcess(CancellationToken cancellationToken)
        {
            while (true)
            {
                UpdateGeneratorEntity();
                var image = ImageToBase64("d:\\36135994_2002047976524612_3088425035963039744_n.jpg");

                await SendMessagesAsync(image);
                await Task.Delay(TimeSpan.FromSeconds(_generatorModel.Delay));
                if (cancellationToken.IsCancellationRequested)
                    return;
            }
        }

        private string ImageToBase64(string path)
        {
            var data = File.ReadAllBytes(path);
            return Convert.ToBase64String(data);
        }

        private async Task SendMessagesAsync(string messageBody)
        {
            var activity = new Activity("Queue");
            using (var operation = telemetryClient.StartOperation<DependencyTelemetry>(activity))
            {
                operation.Telemetry.Type = "Queue";
                operation.Telemetry.Data = "Enqueue " + QueueName;

                try
                {
                    var guid = Guid.NewGuid();
                    var message = new Message(Encoding.UTF8.GetBytes(messageBody));
                    message.UserProperties.Add("guid", guid.ToString());
                    
                    await queueClient.SendAsync(message);

                    operation.Telemetry.Success = true;
                }
                catch (Exception exception)
                {
                    telemetryClient.TrackException(exception);
                    operation.Telemetry.Success = false;
                }
                finally
                {
                    telemetryClient.StopOperation(operation);
                    telemetryClient.Flush();
                }
            }
        }

        private void UpdateGeneratorEntity()
        {
            var entity = Find(_guid);
            _generatorModel = entity;
        }

        public GeneratorModel Find(Guid guid)
        {
            using (IDbConnection db = new SqlConnection(SqlServerDatabaseConnectionString))
            {
                return db.Query<GeneratorModel>("Select * From Generators " +
                                              "WHERE Guid = @Guid", new { guid }).SingleOrDefault();
            }
        }
        public int Update(GeneratorModel generator)
        {
            using (IDbConnection db = new SqlConnection(SqlServerDatabaseConnectionString))
            {
                var sqlQuery = "UPDATE Generators SET Delay = @Delay WHERE Guid = @Guid";
                var rowsAffected = db.Execute(sqlQuery, generator);
                return rowsAffected;
            }
        }

        private GeneratorModel Create(GeneratorModel generator)
        {
            using (IDbConnection db = new SqlConnection(SqlServerDatabaseConnectionString))
            {
                var sqlQuery = "INSERT Generators VALUES(@Guid, @Delay)";
                return db.Query<GeneratorModel>(sqlQuery, generator).SingleOrDefault();
            }
        }

        public int Remove(Guid guid)
        {
            using (IDbConnection db = new SqlConnection(SqlServerDatabaseConnectionString))
            {
                return db.Execute("DELETE FROM Generators WHERE Guid = @Guid", new { guid });
            }
        }
    }
}
