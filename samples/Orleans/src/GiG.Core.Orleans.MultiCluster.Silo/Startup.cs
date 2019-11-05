using GiG.Core.Orleans.Clustering.Consul.Extensions;
using GiG.Core.Orleans.Clustering.Extensions;
using GiG.Core.Orleans.Sample.Grains;
using GiG.Core.Orleans.Silo.Dashboard.Extensions;
using GiG.Core.Orleans.Silo.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Hosting;
using Constants = GiG.Core.Orleans.Sample.Contracts.Constants;
using HostBuilderContext = Microsoft.Extensions.Hosting.HostBuilderContext;


namespace GiG.Core.Orleans.MultiCluster.Silo
{
    public static class Startup
    {
        public static void ConfigureServices(IServiceCollection services)
        {            
        }

        public static void ConfigureOrleans(HostBuilderContext ctx, ISiloBuilder builder)
        {
            builder.ConfigureCluster(ctx.Configuration)
                .ConfigureEndpoints()
                .UseMembershipProvider(ctx.Configuration, x =>
                {
                    x.ConfigureConsulClustering(ctx.Configuration);
                })
                .AddAssemblies(typeof(EchoGrain))
                .AddMemoryGrainStorage(Constants.StreamsMemoryStorageName)
                .ConfigureDashboard(ctx.Configuration);
        }
    }
}