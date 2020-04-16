using GiG.Core.Orleans.Streams.Kafka.Tests.Integration.Fixtures;
using GiG.Core.Orleans.Streams.Kafka.Tests.Integration.Internal;
using GiG.Core.Orleans.Streams.Kafka.Tests.Integration.Mocks;
using Orleans.Streams;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Orleans.Streams.Kafka.Tests.Integration.Tests
{
    [Trait("Category", "IntegrationWithDependency")]
    [Collection(ClusterCollection.Collection)]
    public class StreamTopicTests
    {
        private readonly ClusterFixture _fixture;

        public StreamTopicTests(ClusterFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task SubscribeToStream_WithKafkaTopic_MessageReceived()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var semaphore = new SemaphoreSlim(0, 1);

            var streamReceived = false;
            Task Handler(MockRequest request, StreamSequenceToken token = null)
            {
                streamReceived = true;
                semaphore.Release();
            
                return Task.CompletedTask;
            }
            
            var streamProvider = _fixture.ClusterClient.GetStreamProvider(Constants.StreamProviderName);

            var mockRequestStream = streamProvider.GetStream<MockRequest>(guid, nameof(MockRequest));
            await mockRequestStream.SubscribeAsync(Handler);

            var message = new MockRequest();

            // Act
            await mockRequestStream.OnNextAsync(message);
            await semaphore.WaitAsync(2_000);

            // Assert
            Assert.True(streamReceived);
        }
    }
}