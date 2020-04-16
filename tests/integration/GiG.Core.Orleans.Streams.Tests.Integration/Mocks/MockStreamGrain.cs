using GiG.Core.Orleans.Streams.Abstractions;
using GiG.Core.Orleans.Streams.Tests.Integration.Internal;
using Orleans;
using Orleans.Streams;
using System;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Streams.Tests.Integration.Mocks
{
    public interface IMockStreamGrain : IGrainWithGuidKey
    {
    }

    [NamespaceImplicitStreamSubscription(nameof(MockRequest))]
    public class MockStreamGrain : Grain, IMockStreamGrain, IAsyncObserver<MockRequest>
    {
        private IAsyncStream<MockResponse> _mockResponseStream;

        public override Task OnActivateAsync()
        {
            var streamProvider = GetStreamProvider(Constants.StreamProviderName);
            var mockRequestStream = streamProvider.GetStream<MockRequest>(this.GetPrimaryKey(), StreamHelper.GetNamespace(nameof(MockRequest)));
            mockRequestStream.SubscribeAsync(this);
            
            _mockResponseStream = streamProvider.GetStream<MockResponse>(this.GetPrimaryKey(), StreamHelper.GetNamespace("test", nameof(MockResponse)));
            
            return base.OnActivateAsync();
        }
        
        public Task OnNextAsync(MockRequest item, StreamSequenceToken token = null)
        {
            return _mockResponseStream.OnNextAsync(new MockResponse());
        }

        public Task OnCompletedAsync()
        {
            return Task.CompletedTask;
        }

        public Task OnErrorAsync(Exception ex)
        {
            return Task.CompletedTask;
        }
    }
}