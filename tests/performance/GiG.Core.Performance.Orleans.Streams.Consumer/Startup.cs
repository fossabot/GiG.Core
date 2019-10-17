using GiG.Core.Orleans.Client.Extensions;
using GiG.Core.Orleans.Clustering.Consul.Extensions;
using GiG.Core.Orleans.Clustering.Extensions;
using GiG.Core.Orleans.Clustering.Kubernetes.Extensions;
using GiG.Core.Performance.Orleans.Streams.Consumer;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Hosting;
using HostBuilderContext = Microsoft.Extensions.Hosting.HostBuilderContext;
using Constants = GiG.Core.Performance.Orleans.Streams.Contracts.Constants;

namespace GiG.Core.Orleans.Sample.Consumer
{
    public static class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public static void ConfigureServices(HostBuilderContext ctx, IServiceCollection services)
        {
            var configuration = ctx.Configuration;

            // Orleans Client
            services.AddClusterClient((builder, sp) =>
            {
                builder.ConfigureCluster(configuration)
                .UseMembershipProvider(configuration, x =>
                {
                    x.ConfigureConsulClustering(configuration);
                    x.ConfigureKubernetesClustering(configuration);
                })
                .AddAssemblies(typeof(IConsumerGrain))
                .AddSimpleMessageStreamProvider(Constants.SMSProviderName);
            });

        }
    }
}