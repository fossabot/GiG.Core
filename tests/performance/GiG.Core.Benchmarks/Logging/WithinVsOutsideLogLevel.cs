using BenchmarkDotNet.Attributes;
using GiG.Core.Logging.All.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GiG.Core.Benchmarks.Logging
{
    [BenchmarkCategory("Logging")]
    public class WithinVsOutsideLogLevel
    {
        public IHost ConsoleHost { get; private set; }

        [GlobalSetup]
        public void Setup()
        {
            ConsoleHost = new HostBuilder()
                .ConfigureHostConfiguration(builder => builder
                    .AddJsonFile("appsettings.minimumloglevel.json"))
                .ConfigureLogging()
                .Build();
        }

        [Benchmark]
        public void LogOutsideLevel()
        {
            var consoleLogger = ConsoleHost.Services.GetRequiredService<ILogger<WithinVsOutsideLogLevel>>();

            // Log to console
            for (var i = 0; i < 1000; i++)
            {
                consoleLogger.LogInformation("Test Outside Level");
            }
        }

        [Benchmark]
        public void LogWithinLevel()
        {
            var consoleLogger = ConsoleHost.Services.GetRequiredService<ILogger<WithinVsOutsideLogLevel>>();

            // Log to console
            for (var i = 0; i < 1000; i++)
            {
                consoleLogger.LogError("Test Within Level");
            }
        }

        [GlobalCleanup]
        public void TearDown()
        {

        }
    }
}
