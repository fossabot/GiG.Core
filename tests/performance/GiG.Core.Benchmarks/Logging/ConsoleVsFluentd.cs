using BenchmarkDotNet.Attributes;
using GiG.Core.Logging.All.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GiG.Core.Benchmarks.Logging
{
    [BenchmarkCategory("Logging")]
    public class ConsoleVsFluentd
    {
        private IHost _consoleHost;
        private IHost _fluentDHost;

        [Params(1, 30, 500, 1000)]
        public int LogCount { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            _consoleHost = new HostBuilder()
                .ConfigureHostConfiguration(builder => builder
                    .AddJsonFile("appsettings.consolelogger.json"))
                .ConfigureLogging()
                .Build();

            _fluentDHost = new HostBuilder()
                .ConfigureHostConfiguration(builder => builder
                    .AddJsonFile("appsettings.fluentdlogger.json"))
                .ConfigureLogging()
                .Build();
        }

        [Benchmark]
        public void LogToConsole()
        {
            var consoleLogger = _consoleHost.Services.GetRequiredService<ILogger<ConsoleVsFluentd>>();

            // Log to console
            for (var i = 0; i < LogCount; i++)
            {
                consoleLogger.LogInformation("Test Console");
            }
        }

        [Benchmark]
        public void LogToFluentD()
        {
            var fluentDLogger = _fluentDHost.Services.GetRequiredService<ILogger<ConsoleVsFluentd>>();

            // Log to fluentd
            for (var i = 0; i < LogCount; i++)
            {
                fluentDLogger.LogInformation("Test Fluentd");
            }
        }

        [GlobalCleanup]
        public void TearDown()
        {

        }
    }
}