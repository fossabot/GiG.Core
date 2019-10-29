using GiG.Core.DistributedTracing.Abstractions;
using GiG.Core.Logging.All.Extensions;
using GiG.Core.Orleans.Sample.Http.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Sample.Http
{
    internal class Program
    {
        private static async Task<int> Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            var logger = host.Services.GetRequiredService<ILogger<Program>>();

            using (host)
            {
                try
                {
                    await host.StartAsync();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.Message, ex);
                    return 1;
                }
                finally
                {
                    await host.StopAsync();
                }

                return 0;
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices(services => services.TryAddSingleton<ICorrelationContextAccessor, CorrelationContextAccessor>())
                .ConfigureLogging()
                .ConfigureServices(Startup.ConfigureServices);
    }
}