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
        
        private IServiceProvider _serviceProvider;

        internal IHost Host;
        internal IClusterClient ClusterClient;
        internal SemaphoreSlim Lock;

        public async Task InitializeAsync()
        {
            Host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
                .UseOrleans((ctx, x) =>
                {
                    x.UseLocalhostClustering();
                    x.AddMemoryGrainStorageAsDefault();
                    x.AddMemoryGrainStorage(StreamStorageName);
                    x.AddKafkaStreamProvider(StreamProviderName, k =>
                    {
                        k.FromConfiguration(ctx.Configuration);
                        k.AddTopicStream(StreamNamespace, ctx.Configuration);
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

            _serviceProvider = Host.Services;

            ClusterClient = _serviceProvider.GetRequiredService<IClusterClient>();
            
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
           Lock = new SemaphoreSlim(0, 1);
        }
    }
}