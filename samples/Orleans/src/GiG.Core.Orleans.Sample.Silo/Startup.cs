using GiG.Core.HealthChecks.Orleans.Extensions;
using GiG.Core.Orleans.Clustering.Consul.Extensions;
using GiG.Core.Orleans.Clustering.Extensions;
using GiG.Core.Orleans.Clustering.Kubernetes.Extensions;
using GiG.Core.Orleans.Sample.Contracts;
using GiG.Core.Orleans.Sample.Grains;
using GiG.Core.Orleans.Silo.Dashboard.Extensions;
using GiG.Core.Orleans.Silo.Extensions;
using GiG.Core.Orleans.Storage.Npgsql.Extensions;
using GiG.Core.Orleans.Streams.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Hosting;
using HostBuilderContext = Microsoft.Extensions.Hosting.HostBuilderContext;

namespace GiG.Core.Orleans.Sample.Silo
{
    public static class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddStream();
        }

        // This method gets called by the runtime. Use this method to configure Orleans.
        public static void ConfigureOrleans(HostBuilderContext ctx, ISiloBuilder builder)
        {
            builder.ConfigureCluster(ctx.Configuration)
                .ConfigureDashboard(ctx.Configuration)
                .ConfigureEndpoints(ctx.Configuration)
                .UseMembershipProvider(ctx.Configuration, x =>
                {
                    x.ConfigureConsulClustering(ctx.Configuration);
                    x.ConfigureKubernetesClustering(ctx.Configuration);
                })
                .AddHealthCheckDependencies()
                .AddNpgsqlGrainStorage(Constants.StorageProviderName, ctx.Configuration)
                .AddAssemblies(typeof(WalletGrain))
                .AddSimpleMessageStreamProvider(Constants.StreamProviderName)
                .UseSignalR()
                .AddMemoryGrainStorage(Constants.StreamsMemoryStorageName);
        }
    }
}