using GiG.Core.Hosting.Extensions;
using GiG.Core.Logging.All.Extensions;
using GiG.Core.Orleans.Clustering.Consul.Silo.Extensions;
using GiG.Core.Orleans.Hosting.Silo.Extensions;
using GiG.Core.Orleans.Sample.Grains;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Orleans.Hosting;
using System;
using System.IO;
using HostBuilderContext = Microsoft.Extensions.Hosting.HostBuilderContext;

namespace GiG.Core.Orleans.Sample.Silo
{
    public class Program
    {
        public static void Main()
        {
            new HostBuilder()
                .ConfigureServices(services => services.AddCorrelationAccessor())
                .ConfigureHostConfiguration(ConfigureApplicationConfiguration())
                .ConfigureLogging()
                .UseOrleans(ConfigureOrleans)
                .Build()
                .Run();
        }

        private static Action<IConfigurationBuilder> ConfigureApplicationConfiguration()
        {
            return builder => builder
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
        }

        private static void ConfigureOrleans(HostBuilderContext ctx, ISiloBuilder builder)
        {
            builder.ConfigureCluster(ctx.Configuration)
                .ConfigureDashboard(ctx.Configuration)
                .ConfigureEndpoint()
                .ConfigureConsulClustering(ctx.Configuration)
                .AddAssemblies(typeof(TransactionGrain));
        }
    }
}