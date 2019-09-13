using GiG.Core.Logging.All.Extensions;
using GiG.Core.Orleans.Hosting.Extensions;
using GiG.Core.Orleans.Sample.Grains;
using GiG.Core.Orleans.Silo.Clustering.Consul.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.IO;
using GiG.Core.DistributedTracing.Orleans.Extensions;

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
                        .ConfigureCluster(ctx.Configuration)
                        .ConfigureDashboard(ctx.Configuration)
                        .ConfigureEndpoint()
                        .ConfigureConsulClustering(ctx.Configuration)
                        .AddAssemblies(typeof(TransactionGrain));
                })
                .ConfigureHostConfiguration(builder => builder
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddEnvironmentVariables())
                .ConfigureServices(services => services.AddCorrelationAccessor())
                .ConfigureLogging()
                .Build()
                .Run();
        }
    }
}