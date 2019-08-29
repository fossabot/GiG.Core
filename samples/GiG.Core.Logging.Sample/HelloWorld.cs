﻿using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GiG.Core.Logging.Sample
{
    public class HelloWorld : IHostedService
    {
        private ILogger _logger;

        public HelloWorld(ILogger<HelloWorld> logger)
        {
            _logger = logger;
        }      

        public Task StartAsync(CancellationToken cancellationToken)
        {
            LogToConsole();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private void LogToConsole()
        {
            _logger.LogWarning("Stating Up");
            Console.WriteLine("Hello World");
        }
   
    }
}
