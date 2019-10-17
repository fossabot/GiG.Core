using GiG.Core.Orleans.Clustering.Consul.Configurations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using GiG.Core.Orleans.Client.Extensions;
using GiG.Core.Orleans.Clustering.Consul.Extensions;
using GiG.Core.Performance.Orleans.Streams.Producer;
using Orleans;
using System.Threading.Tasks;

namespace GiG.Core.Performance.Orleans.Streams.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var clientHost = Host.CreateDefaultBuilder()
                .ConfigureServices((ctx, services) =>
                {
                    services.Configure<ConsulOptions>(ctx.Configuration);

                    services.AddClusterClient(x =>
                    {
                        x.ConfigureCluster(ctx.Configuration);
                        x.ConfigureConsulClustering(ctx.Configuration);
                        x.AddAssemblies(typeof(IProducerGrain));
                    });
                })
                .Build();

            var serviceProvider = clientHost.Services;

            var clusterClient = serviceProvider.GetRequiredService<IClusterClient>();

            var grain = clusterClient.GetGrain<SMSProviderProducerGrain>(new System.Guid());
            await grain.ProduceAsync("TestHeader", "TestBody");
        }
    }
}
