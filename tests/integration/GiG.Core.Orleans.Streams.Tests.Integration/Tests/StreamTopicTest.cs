using GiG.Core.Orleans.Streams.Tests.Integration.Fixtures;
using GiG.Core.Orleans.Streams.Tests.Integration.Mocks;
using Orleans.Streams;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Orleans.Streams.Tests.Integration.Tests
{
    [Trait("Category", "IntegrationWithDependency")]
    public class StreamTopicTest : IClassFixture<ClusterFixture>
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
            
            Thread.Sleep(2000);

            // Assert
            Assert.True(_streamReceived);
        }

        private Task Handler(MockMessage message, StreamSequenceToken token = null)
        {
            _streamReceived = true;
            return Task.CompletedTask;
        }
    }
}