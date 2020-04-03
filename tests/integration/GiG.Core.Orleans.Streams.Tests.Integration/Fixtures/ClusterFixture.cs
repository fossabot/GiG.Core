using GiG.Core.Orleans.Streams.Extensions;
using GiG.Core.Orleans.Streams.Kafka.Extensions;
using GiG.Core.Orleans.Streams.Tests.Integration.Internal;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans;
using Orleans.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Orleans.Streams.Tests.Integration.Fixtures
{
    [CollectionDefinition(Collection)]
    public class ClusterCollection : ICollectionFixture<ClusterFixture>
    {
        public const string Collection = "InMemory Cluster collection";
    }
    
    public class ClusterFixture : IAsyncLifetime
    {
        private const string StreamStorageName = "PubSubStore";
        private const string StreamProviderName = "KafkaStreamProvider";
        private const string StreamNamespace = "TestStream";
        
        internal IHost Host;
        private IServiceProvider ServiceProvider;
        internal IClusterClient ClusterClient;
        internal SemaphoreSlim _lock;

        public async Task InitializeAsync()
        {
            Host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
                .UseOrleans((ctx, x) =>
                {
                    x.UseLocalhostClustering();
                    x.AddMemoryGrainStorageAsDefault();
                    x.AddMemoryGrainStorage(StreamStorageName);
                    x.AddKafkaStreamProvider(StreamProviderName, x =>
                    {
                        x.FromConfiguration(ctx.Configuration);
                        x.AddTopicStream(StreamNamespace, ctx.Configuration);
                    });
                })
                .ConfigureWebHostDefaults(x =>
                {
                    x.UseTestServer();
                    x.UseStartup<Startup>();
                })
                .ConfigureServices(x =>
                {
                    x.AddStream();
                })
                .Build();

            await Host.StartAsync();

            ServiceProvider = Host.Services;

            ClusterClient = ServiceProvider.GetRequiredService<IClusterClient>();
            
            InitializeWait();
        }

        public async Task DisposeAsync()
        {
            await ClusterClient.Close();

            await Host.StopAsync();
            Host.Dispose();
        }

        private void InitializeWait()
        {
           _lock = new SemaphoreSlim(0, 1);
        }
    }
}