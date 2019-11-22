using GiG.Core.DistributedTracing.MassTransit.Extensions;
using GiG.Core.Messaging.MassTransit.Extensions;
using GiG.Core.Orleans.Client.Extensions;
using GiG.Core.Orleans.Clustering.Consul.Extensions;
using GiG.Core.Orleans.Clustering.Extensions;
using GiG.Core.Orleans.Clustering.Kubernetes.Extensions;
using GiG.Core.Orleans.Sample.Consumer.Extensions;
using GiG.Core.Orleans.Sample.Contracts;
using GiG.Core.Orleans.Streams.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Hosting;
using Constants = GiG.Core.Orleans.Sample.Contracts.Constants;
using HostBuilderContext = Microsoft.Extensions.Hosting.HostBuilderContext;

namespace GiG.Core.Orleans.Sample.Consumer
{
    public static class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public static void ConfigureServices(HostBuilderContext ctx, IServiceCollection services)
        {
            var configuration = ctx.Configuration;

            services.AddMassTransitContext();
            services.AddCorrelationAccessor();
            services.AddStream();

            services.AddMessageConsumer(configuration);
            
            // Orleans Client
            services.AddDefaultClusterClient((builder, sp) =>
            {
                builder.ConfigureCluster(configuration)
                .UseMembershipProvider(configuration, x =>
                {
                    x.ConfigureConsulClustering(configuration);
                    x.ConfigureKubernetesClustering(configuration);
                })
                .AddAssemblies(typeof(IWalletGrain))
                .AddSimpleMessageStreamProvider(Constants.StreamProviderName);
            });
        }
    }
}