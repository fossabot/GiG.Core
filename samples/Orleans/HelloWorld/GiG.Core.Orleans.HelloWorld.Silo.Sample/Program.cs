using GiG.Core.Logging.All.Extensions;
using GiG.Core.Orleans.Hosting.Extensions;
using GiG.Core.Orleans.Samples.HelloWorld.Grains;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Orleans.Hosting;
using System.IO;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Sample.HelloWorld.Silo
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                .UseOrleans((ctx, builder) =>
                {
                    builder
                        .UseLocalhostClustering()
                        .ConfigureCluster(ctx.Configuration)
                        .ConfigureDashboard(ctx.Configuration)
                        .ConfigureEndpoint()
                        .AddAssemblies(typeof(HelloGrain).Assembly);
                })
                .ConfigureHostConfiguration(builder => builder
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true))
                .ConfigureLogging()
                .Build();

            await host.RunAsync();
        }
    }
}
