using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MonitoringServer.Models;

namespace MonitoringServer.Infastructure
{
    public class TimedHostedService : IHostedService, IDisposable
    {
        public static IDictionary<string, HandlerViewModel> HandlerStatuses { get; set; }
        private readonly ILogger _logger;
        private Timer _timer;
        private SubscriptionClientHandlers _subscriptionClient;

        public TimedHostedService(ILogger<TimedHostedService> logger)
        {
            HandlerStatuses = new Dictionary<string, HandlerViewModel>();
            _logger = logger;
            _subscriptionClient = new SubscriptionClientHandlers(HandlerStatuses);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is starting.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(30));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            _logger.LogInformation("Timed Background Service is working.");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
