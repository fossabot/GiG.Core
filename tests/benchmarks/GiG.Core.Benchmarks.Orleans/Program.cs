using BenchmarkDotNet.Running;
using GiG.Core.Benchmarks.Orleans.Streams;
using GiG.Core.Configuration.Extensions;
using GiG.Core.Context.Orleans.Extensions;
using GiG.Core.DistributedTracing.Orleans.Extensions;
using GiG.Core.Hosting.Extensions;
using GiG.Core.Logging.All.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans;
using System.Threading;

namespace GiG.Core.Benchmarks.Orleans
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            host.Start();

            var clientClient = host.Services.GetRequiredService<IClusterClient>();

            // Waiting until Silo is ready to serve clients
            while (!clientClient.IsInitialized)
            {
                Thread.Sleep(250);
            }

            BenchmarkSwitcher.FromAssembly(typeof(StreamsBenchmark).Assembly).Run(args);
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseApplicationMetadata()
                .ConfigureServices(services =>
                {
                    services.AddCorrelationAccessor();
                    services.AddRequestContextAccessor();
                })
                .ConfigureExternalConfiguration()
                .ConfigureLogging()
                .UseOrleans(SiloStartup.ConfigureOrleans);
    }
}