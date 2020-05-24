using GiG.Core.Orleans.Streams.Abstractions;
using GiG.Core.Orleans.Streams.Tests.Integration.Fixtures;
using GiG.Core.Orleans.Streams.Tests.Integration.Internal;
using GiG.Core.Orleans.Streams.Tests.Integration.Mocks;
using Orleans.Streams;
using System;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Orleans.Streams.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    [Collection(ClusterCollection.Collection)]
    public class StreamTopicTests
    {
        private readonly ClusterFixture _fixture;

        public StreamTopicTests(ClusterFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task SubscribeToStream_WithNamespaceImplicitSubscription_MessageReceived()
        {
            // Arrange
            var guid = Guid.NewGuid();
            const string namespacePrefix = "test-prefix";
            var mockRequestNamespace = $"{namespacePrefix}.{nameof(MockRequest)}";
            var mockResponseNamespace = $"{namespacePrefix}.orleans.test.{nameof(MockResponse)}.v1";

            var streamReceived = false;
            Task Handler(MockResponse request, StreamSequenceToken token = null)
            {
                streamReceived = true;
            
                return Task.CompletedTask;
            }
            
            var streamProvider = _fixture.ClusterClient.GetStreamProvider(Constants.StreamProviderName);
            var mockRequestStream = streamProvider.GetStream<MockRequest>(guid, StreamHelper.GetNamespace(nameof(MockRequest)));
            var mockResponseStream = streamProvider.GetStream<MockResponse>(guid, StreamHelper.GetNamespace("test", nameof(MockResponse)));
            await mockResponseStream.SubscribeAsync(Handler);

            var message = new MockRequest();

            // Act
            await mockRequestStream.OnNextAsync(message);

            // Assert
            Assert.True(streamReceived);
            Assert.Equal(namespacePrefix, StreamHelper.NamespacePrefix);
            Assert.Equal(mockRequestNamespace, StreamHelper.GetNamespace(nameof(MockRequest)));
            Assert.Equal(mockResponseNamespace, StreamHelper.GetNamespace("test", nameof(MockResponse)));
        }
    }
}