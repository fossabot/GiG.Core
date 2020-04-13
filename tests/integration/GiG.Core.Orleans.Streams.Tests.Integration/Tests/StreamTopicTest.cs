using GiG.Core.Orleans.Streams.Tests.Integration.Fixtures;
using GiG.Core.Orleans.Streams.Tests.Integration.Mocks;
using Orleans.Streams;
using System;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Orleans.Streams.Tests.Integration.Tests
{
    [Trait("Category", "IntegrationWithDependency")]
    [Collection(ClusterCollection.Collection)]
    public class StreamTopicTest
    {
        private readonly ClusterFixture _fixture;
        private IAsyncStream<MockMessage> _stream;
        private const string StreamProviderName = "KafkaStreamProvider";
        private const string StreamNamespace = "TestStream";
        private bool _streamReceived;


        public StreamTopicTest(ClusterFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task SubscribeToStream_WithKafkaTopic_MessageReceived()
        {
            // Arrange
            var guid = Guid.NewGuid();

            var streamProvider = _fixture.ClusterClient.GetStreamProvider(StreamProviderName);

            _stream = streamProvider.GetStream<MockMessage>(guid, StreamNamespace);
            await _stream.SubscribeAsync(Handler);

            var message = new MockMessage();

            // Act
            await _stream.OnNextAsync(message);

            await _fixture.Lock.WaitAsync(2000);

            // Assert
            Assert.True(_streamReceived);
        }

        private Task Handler(MockMessage message, StreamSequenceToken token = null)
        {
            _streamReceived = true;
            
            _fixture.Lock.Release();
            
            return Task.CompletedTask;
        }
    }
}