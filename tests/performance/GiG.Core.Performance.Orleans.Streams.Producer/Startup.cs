using GiG.Core.Orleans.Clustering.Consul.Extensions;
using GiG.Core.Orleans.Clustering.Extensions;
using GiG.Core.Orleans.Clustering.Kubernetes.Extensions;
using GiG.Core.Orleans.Hosting.Silo.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Hosting;
using Orleans.Streams.Kafka.Config;
using Constants = GiG.Core.Performance.Orleans.Streams.Contracts.Constants;
using HostBuilderContext = Microsoft.Extensions.Hosting.HostBuilderContext;

namespace GiG.Core.Performance.Orleans.Streams.Producer
{
    public static class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddHostedService<HostedService>();
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
                .AddMemoryGrainStorageAsDefault()
                .AddAssemblies(typeof(ProducerGrain))
                .AddSimpleMessageStreamProvider(Constants.SMSProviderName)
                .AddMemoryGrainStorage(Constants.StreamsMemoryStorageName)
                .AddKafka(Constants.KafkaProviderName)
                .WithOptions(options =>
                {
                    options.BrokerList = new[] { "kafka:9092" };
                    options.ConsumerGroupId = "E2EGroup";
                    options.ConsumeMode = ConsumeMode.StreamEnd;

                    options
                        .AddTopic(Constants.MessageNamespace);
                })
                .AddJson()
                .Build();
        }
    }
}