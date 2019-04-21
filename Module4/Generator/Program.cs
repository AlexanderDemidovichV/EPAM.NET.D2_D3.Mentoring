﻿using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure.ServiceBus;

namespace Generator
{
    class Program
    {
        private static readonly TelemetryClient telemetryClient = 
            new TelemetryClient(new TelemetryConfiguration("7da04e81-63ca-4b0b-9492-a6b2caf0df53"));

        private static IQueueClient queueClient;

        private const string ServiceBusConnectionString =
            "Endpoint=sb://dzemidovich-dev.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=41JPYUS11Yx8b71bXdyLpoGsfsWRvNrIrBjkYYeTRS4=";

        private const string SqlServerDatabaseConnectionString =
            "Server=tcp:dzemidovich-sql-server-dev.database.windows.net,1433;Initial Catalog=epam-mentoring;Persist Security Info=False;User ID=Gendalf;Password=AzureMentoringEpam2018;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        private const string QueueName = "images";

        static async Task Main()
        {
            queueClient = new QueueClient(ServiceBusConnectionString, QueueName);

            while (true)
            {
                var image = ImageToBase64("C:\\Users\\Aliaksandr_Dzemidovi\\Desktop\\36135994_2002047976524612_3088425035963039744_n.jpg");

                await SendMessagesAsync(image);
                await Task.Delay(TimeSpan.FromSeconds(5));
            }
        }

        private static string ImageToBase64(string path)
        {
            var data = File.ReadAllBytes(path);
            return Convert.ToBase64String(data);
        }

        private static async Task SendMessagesAsync(string messageBody)
        {
            var activity = new Activity("Queue");
            using (var operation = telemetryClient.StartOperation<DependencyTelemetry>(activity))
            {
                operation.Telemetry.Type = "Queue";
                operation.Telemetry.Data = "Enqueue " + QueueName;

                try
                {
                    var guid = Guid.NewGuid();
                    // Create a new message to send to the queue
                    var message = new Message(Encoding.UTF8.GetBytes(messageBody));
                    //message.MessageId = "";
                    message.UserProperties.Add("guid", guid.ToString());

                    // Write the body of the message to the console
                    Console.WriteLine($"Sending message: {guid}");

                    // Send the message to the queue
                    await queueClient.SendAsync(message);

                    operation.Telemetry.Success = true;
                }
                catch (Exception exception)
                {
                    telemetryClient.TrackException(exception);
                    // Set operation.Telemetry Success and ResponseCode here.
                    operation.Telemetry.Success = false;
                    Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
                }
                finally
                {
                    telemetryClient.StopOperation(operation);
                    telemetryClient.Flush();
                }
            }
        }

        public static GeneratorModel Find(Guid guid)
        {
            using (IDbConnection db = new SqlConnection(SqlServerDatabaseConnectionString))
            {
                return db.Query<GeneratorModel>("Select * From Generators " +
                                              "WHERE Guid = @Guid", new { guid }).SingleOrDefault();
            }
        }
        public static int Update(GeneratorModel generator)
        {
            using (IDbConnection db = new SqlConnection(SqlServerDatabaseConnectionString))
            {
                var sqlQuery = "UPDATE Generators SET Delay = @Delay WHERE Guid = @Guid";
                var rowsAffected = db.Execute(sqlQuery, generator);
                return rowsAffected;
            }
        }

        public static int Create(GeneratorModel generator)
        {
            using (IDbConnection db = new SqlConnection(SqlServerDatabaseConnectionString))
            {
                var sqlQuery = "INSERT Generators VALUES(@Guid, @Delay)";
                var rowsAffected = db.Execute(sqlQuery, generator);
                return rowsAffected;
            }
        }

        public static int Remove(Guid guid)
        {
            using (IDbConnection db = new SqlConnection(SqlServerDatabaseConnectionString))
            {
                return db.Execute("DELETE FROM Generators WHERE Guid = @Guid", new { guid });
            }
        }
    }
}
