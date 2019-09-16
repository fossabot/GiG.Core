using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GiG.Core.Logging.Sample
{
    public class HelloWorldService : BackgroundService
    {
        private readonly ILogger _logger;
        private Timer _timer;

        public HelloWorldService(ILogger<HelloWorldService> logger)
        {
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(LogToConsole, null, 0, 1000);
            Console.WriteLine("Starting up");
            
            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _logger.LogInformation("Shutting down");

            base.Dispose();
            _timer?.Dispose();
        }

        private void LogToConsole(object state)
        {
            Console.WriteLine("Logging...");
            _logger.LogTrace("Hello World! - Trace");
            _logger.LogDebug("Hello World! - Debug");
            _logger.LogInformation("Hello World! - Information");
            _logger.LogWarning("Hello World! - Warning");
            _logger.LogError("Hello World! - Error");
            _logger.LogCritical("Hello World! - Critical");
        }
    }
}