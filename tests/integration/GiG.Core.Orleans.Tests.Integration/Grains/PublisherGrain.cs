using GiG.Core.Context.Abstractions;
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
        private readonly IMessagePublisher<MockMessage> _messagePublisher;

        public PublisherGrain(IMessagePublisher<MockMessage> messagePublisher)
        {
            _messagePublisher = messagePublisher;
        }

        public override async Task OnActivateAsync()
        {
            var streamProvider = GetStreamProvider("SMSProvider");

            _messagePublisher.SetAsyncStream(
                streamProvider.GetStream<MockMessage>(this.GetPrimaryKey(), "MockMessageNamespace"));

            await base.OnActivateAsync();
        }

        public async Task<Guid> PublishMessage(MockMessage mockMessage)
        {
            await _messagePublisher.PublishEventAsync(mockMessage);

            return RequestContext.ActivityId;
        }
    }
}