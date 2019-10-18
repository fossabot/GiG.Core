using GiG.Core.Orleans.Streams.Abstractions;
using GiG.Core.Orleans.Tests.Integration.Contracts;
using GiG.Core.Orleans.Tests.Integration.Mocks;
using Orleans;
using Orleans.Runtime;
using System;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Tests.Integration.Grains
{
    public class PublisherGrain : Grain, IPublisherGrain
    {
        private readonly IStreamFactory _streamFactory;
        private IStream<MockMessage> _stream;

        public PublisherGrain(IStreamFactory streamFactory)
        {
            _streamFactory = streamFactory;
        }

        public override async Task OnActivateAsync()
        {
            var streamProvider = GetStreamProvider("SMSProvider");

            _stream = _streamFactory.GetStream<MockMessage>(streamProvider, this.GetPrimaryKey(), "MockMessageNamespace");

            await base.OnActivateAsync();
        }

        public async Task<Guid> PublishMessage(MockMessage mockMessage)
        {
            await _stream.PublishAsync(mockMessage);

            return RequestContext.ActivityId;
        }
    }
}