using GiG.Core.Logging.All.Extensions;
using GiG.Core.Orleans.Hosting.Extensions;
using GiG.Core.Orleans.Sample.Grains;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Orleans.Hosting;
using System.IO;

namespace GiG.Core.Orleans.Sample.Silo
{
    public class Program
    {
        public static void Main()
        {
            new HostBuilder()
                .UseOrleans((ctx, builder) =>
                {
                    builder
                        .UseLocalhostClustering()
                        .ConfigureCluster(ctx.Configuration)
                        .ConfigureDashboard(ctx.Configuration)
                        .ConfigureEndpoint()
                        .AddAssemblies(typeof(TransactionGrain));
                })
                .ConfigureHostConfiguration(builder => builder
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddEnvironmentVariables())
                .ConfigureLogging()
                .Build()
                .Run();
        }
    }
}