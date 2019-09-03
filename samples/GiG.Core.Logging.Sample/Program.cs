using System.IO;
using GiG.Core.Extensions.Logging;
using GiG.Core.Extensions.Logging.Enrichers;
using GiG.Core.Extensions.Logging.Sinks.Console;
using GiG.Core.Extensions.Logging.Sinks.Fluentd;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace GiG.Core.Logging.Sample
{
    static class Program
    {
        public static void Main()
        {
            CreateHostBuilder().Build().Run();
        }

        private static IHostBuilder CreateHostBuilder()
        {
            return new HostBuilder()
                .ConfigureHostConfiguration(builder => builder
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true))
                .UseLogging(x => x
                    .WriteToConsole()
                    .WriteToFluentd()
                    .EnrichWithApplicationName())
                .ConfigureServices(Startup.ConfigureServices);
        }
    }
}