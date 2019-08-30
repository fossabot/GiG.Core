using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GiG.Core.Logging.Sample
{
    public class HelloWorld : IHostedService
    {
        private readonly ILogger _logger;

        public HelloWorld(ILogger<HelloWorld> logger) => _logger = logger;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            LogToConsole();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        private void LogToConsole()
        {
            _logger.LogWarning("Starting Up");
            Console.WriteLine("Hello World");
        }
    }
}
