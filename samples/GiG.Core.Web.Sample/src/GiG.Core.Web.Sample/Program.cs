using GiG.Core.Extensions.Configuration;
using GiG.Core.Extensions.DistributedTracing.Web;
using GiG.Core.Extensions.Logging;
using GiG.Core.Extensions.Logging.Enrichers;
using GiG.Core.Extensions.Logging.Sinks.Console;
using GiG.Core.Extensions.Logging.Sinks.Fluentd;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace GiG.Core.Web.Sample
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureExternalConfiguration()
                .ConfigureServices(x => x.AddCorrelationId())
                .ConfigureLogging(x => x
                    .WriteToConsole()
                    .WriteToFluentd()
                    .EnrichWithApplicationMetadata()
                    .EnrichWithCorrelationId())
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
    }
}