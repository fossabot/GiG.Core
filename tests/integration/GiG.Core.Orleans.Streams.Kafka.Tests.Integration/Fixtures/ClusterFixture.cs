using GiG.Core.Orleans.Streams.Kafka.Extensions;
using GiG.Core.Orleans.Streams.Kafka.Tests.Integration.Internal;
using GiG.Core.Orleans.Streams.Kafka.Tests.Integration.Mocks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans;
using Orleans.Hosting;
using System;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Orleans.Streams.Kafka.Tests.Integration.Fixtures
{
    [CollectionDefinition(Collection)]
    public class ClusterCollection : ICollectionFixture<ClusterFixture>
    {
        public const string Collection = "InMemory Cluster collection";
    }
    
    public class ClusterFixture : IAsyncLifetime
    {
        private const string StreamStorageName = "PubSubStore";
        
        private IServiceProvider _serviceProvider;

        internal IHost Host;
        internal IClusterClient ClusterClient;

        public async Task InitializeAsync()
        {
            Host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
                .UseOrleans((ctx, x) =>
                {
                    x.UseLocalhostClustering();
                    x.AddMemoryGrainStorage(StreamStorageName);
                    x.AddKafkaStreamProvider(Constants.StreamProviderName, k =>
                    {
                        k.FromConfiguration(ctx.Configuration);
                        k.AddTopicStream(nameof(MockRequest), ctx.Configuration);
                    });
                })
                .ConfigureWebHostDefaults(x =>
                {
                    x.UseTestServer();
                    x.UseStartup<Startup>();
                })
                .Build();

            await Host.StartAsync();

            _serviceProvider = Host.Services;

            ClusterClient = _serviceProvider.GetRequiredService<IClusterClient>();
        }

        public async Task DisposeAsync()
        {
            await ClusterClient.Close();

            await Host.StopAsync();
            Host.Dispose();
        }
    }
}