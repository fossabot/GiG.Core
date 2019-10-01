using GiG.Core.Orleans.Clustering.Silo.Extensions;
using GiG.Core.Orleans.Clustering.Consul.Silo.Extensions;
using GiG.Core.Orleans.Clustering.Kubernetes.Silo.Extensions;
using GiG.Core.Orleans.Hosting.Silo.Extensions;
using GiG.Core.Orleans.Sample.Contracts;
using GiG.Core.Orleans.Sample.Grains;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Hosting;
using HostBuilderContext = Microsoft.Extensions.Hosting.HostBuilderContext;

namespace GiG.Core.Orleans.Sample.Silo
{
    public static class Startup
    {
        private const string StreamProviderName = "SMSProvider";
        
        // This method gets called by the runtime. Use this method to add services to the container.
        public static void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure Orleans.
        public static void ConfigureOrleans(HostBuilderContext ctx, ISiloBuilder builder)
        {
            builder.ConfigureCluster(ctx.Configuration)
                .ConfigureDashboard(ctx.Configuration)
                .ConfigureEndpoints()
                .UseMembershipProvider(ctx.Configuration, x =>
                {
                    x.ConfigureConsulClustering(ctx.Configuration);
                    x.ConfigureKubernetesClustering(ctx.Configuration);
                })
                .AddAssemblies(typeof(TransactionGrain))
                .AddSimpleMessageStreamProvider(Constants.StreamProviderName)
                .AddMemoryGrainStorage(Constants.StreamsMemoryStorageName);
        }
    }
}