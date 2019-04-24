using System;
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
        private TelemetryClient _telemetryClient;

        private IQueueClient queueClient;

        private const string ServiceBusConnectionString =
            "Endpoint=sb://dzemidovich-dev.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=41JPYUS11Yx8b71bXdyLpoGsfsWRvNrIrBjkYYeTRS4=";
        
        private const string InstrumentationKey = "3d240292-a095-4b96-b7f9-23cc49cd21f7";

        private const string QueueName = "images";

        private readonly Guid _guid;

        private static GeneratorModel _generatorModel;

        private readonly CancellationTokenSource _cancellationTokenSource;

        private Task _processTask;

        private readonly string _inDirectory;

        public GeneratorService(string inDirectory)
        {
            _inDirectory = inDirectory;
            _guid = Guid.NewGuid();
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public void Start()
        {
            queueClient = new QueueClient(ServiceBusConnectionString, QueueName);

            _telemetryClient = new TelemetryClient(
                new TelemetryConfiguration(InstrumentationKey));
            _generatorModel = GeneratorHelper.Create(new GeneratorModel
            {
                Guid = _guid,
                // Default value
                Delay = 30 
            });

            _processTask = StartProcess(_cancellationTokenSource.Token);
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
            _processTask.Wait();
            _telemetryClient.Flush();
            GeneratorHelper.Remove(_generatorModel.Guid);
            queueClient.CloseAsync().GetAwaiter().GetResult();
        }

        private async Task StartProcess(CancellationToken cancellationToken)
        {
            while (true)
            {
                UpdateGeneratorEntity();
                var directory = new DirectoryInfo(_inDirectory);

                var random = new Random();
                var list = directory.GetFiles("*.jpg").ToList();
                var index = random.Next(list.Count);

                var image = ImageToBase64(list[index].FullName);

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
            using (var operation = _telemetryClient.StartOperation<DependencyTelemetry>(activity))
            {
                operation.Telemetry.Type = "Queue";
                operation.Telemetry.Data = "Enqueue " + QueueName;

                try
                {
                    var message = new Message(Encoding.UTF8.GetBytes(messageBody));
                    message.UserProperties.Add("guid", _guid.ToString());
                    
                    await queueClient.SendAsync(message);

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

        private void UpdateGeneratorEntity()
        {
            var entity = GeneratorHelper.Find(_guid);
            _generatorModel = entity;
        }
    }
}
