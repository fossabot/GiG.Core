using GiG.Core.Orleans.Client.Extensions;
using GiG.Core.Orleans.Clustering.Consul.Extensions;
using GiG.Core.Orleans.Clustering.Extensions;
using GiG.Core.Orleans.Clustering.Kubernetes.Extensions;
using GiG.Core.Orleans.Sample.Consumer.Extensions;
using GiG.Core.Orleans.Sample.Grains.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GiG.Core.Orleans.Sample.Consumer
{
    public static class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public static void ConfigureServices(HostBuilderContext ctx, IServiceCollection services)
        {
            var configuration = ctx.Configuration;
            
            services.AddMessageConsumer(configuration);
            
            // Orleans Client
            services.AddClusterClient((builder, sp) =>
            {
                builder.ConfigureCluster(configuration);
                builder.UseMembershipProvider(configuration, x =>
                {
                    x.ConfigureConsulClustering(configuration);
                    x.ConfigureKubernetesClustering(configuration);
                });
                builder.AddAssemblies(typeof(IPaymentGrain));
            });

        }
    }
}