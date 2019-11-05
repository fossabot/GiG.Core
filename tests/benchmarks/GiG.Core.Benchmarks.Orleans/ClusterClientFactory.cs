using GiG.Core.Benchmarks.Orleans.Streams.Contracts;
using GiG.Core.Orleans.Client.Extensions;
using GiG.Core.Orleans.Streams.Kafka.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans;
using Orleans.Hosting;
using Orleans.Streams.Kafka.Config;

namespace GiG.Core.Benchmarks.Orleans
{
    public static class ClusterClientFactory
    {
        public static IClusterClient Create()
        {
            var host = Host.CreateDefaultBuilder()             
                .ConfigureServices(ConfigureServices)
                .Build();

            return host.Services.GetRequiredService<IClusterClient>();
        }

        private static void ConfigureServices(Microsoft.Extensions.Hosting.HostBuilderContext ctx, IServiceCollection services)
        {
            // Orleans Client
            services.AddDefaultClusterClient((builder, sp) =>
            {
                builder.UseLocalhostClustering()
                .AddAssemblies(typeof(ISMSProviderProducerGrain))
                .AddSimpleMessageStreamProvider(Constants.SMSProviderName)
                .AddKafka(Constants.KafkaProviderName)
                .WithOptions(options =>
                {
                    options.FromConfiguration(ctx.Configuration);
                    options.ConsumeMode = ConsumeMode.StreamStart;

                    options
                        .AddTopic(Constants.MessageNamespace);
                })
                .AddJson()
                .Build();
            });
        }
    }
}
