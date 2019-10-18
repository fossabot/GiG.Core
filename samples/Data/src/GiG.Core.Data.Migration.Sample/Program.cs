using GiG.Core.Logging.All.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GiG.Core.Data.Migration.Sample
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
                .ConfigureLogging()
                .ConfigureServices(Startup.ConfigureServices);
    }
}
