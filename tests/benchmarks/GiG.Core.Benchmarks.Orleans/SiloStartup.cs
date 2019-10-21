using GiG.Core.Benchmarks.Orleans.Streams.Grains;
using GiG.Core.Orleans.Hosting.Silo.Extensions;
using GiG.Core.Orleans.Streams.Kafka.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Hosting;
using Orleans.Streams.Kafka.Config;
using HostBuilderContext = Microsoft.Extensions.Hosting.HostBuilderContext;

namespace GiG.Core.Benchmarks.Orleans
{
    public static class SiloStartup
    {
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
                .UseLocalhostClustering()
                .AddMemoryGrainStorageAsDefault()
                .AddAssemblies(typeof(ProducerGrain))
                .AddSimpleMessageStreamProvider(Constants.SMSProviderName)
                .AddMemoryGrainStorage(Constants.StreamsMemoryStorageName)
                .AddKafka(Constants.KafkaProviderName)
                .WithOptions(options =>
                {
                    options.FromConfiguration(ctx.Configuration);
                    options.ConsumeMode = ConsumeMode.StreamEnd;

                    options.AddTopic(Constants.MessageNamespace);
                })
                .AddJson()
                .Build();
        }
    }
}