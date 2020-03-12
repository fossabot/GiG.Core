using System.Threading.Tasks;
using GiG.Core.Orleans.Tests.Integration.Contracts;
using GiG.Core.Orleans.Tests.Integration.Mocks;
using Orleans;
using Orleans.Streams;

namespace GiG.Core.Orleans.Tests.Integration.Grains
{
    [ImplicitStreamSubscription(MockCommand.MockCommandNamespace)]
    public class CommandTestGrain : Grain, ICommandTestGrain
    {
        private IAsyncStream<MockCommand> _incomingStream;
        private IStreamProvider _streamProvider;

        public override async Task OnActivateAsync()
        {
            _streamProvider = GetStreamProvider(Constants.StreamProviderName);
            _incomingStream = _streamProvider.GetStream<MockCommand>(this.GetPrimaryKey(), MockCommand.MockCommandNamespace);
            await _incomingStream.SubscribeAsync(OnNextAsync);

            await base.OnActivateAsync();
        }

        private async Task OnNextAsync(MockCommand mockCommand, StreamSequenceToken sequenceToken = null)
        {
            if (mockCommand.IsSuccess)
            {
                var mockSuccessEventStream = _streamProvider.GetStream<MockSuccessEvent>(this.GetPrimaryKey(), MockSuccessEvent.MockSuccessEventNamespace);
                await mockSuccessEventStream.OnNextAsync(new MockSuccessEvent
                {
                    GrainId = this.GetPrimaryKey()
                });
            }
            else
            {
                var mockFailureEventStream = _streamProvider.GetStream<MockFailureEvent>(this.GetPrimaryKey(), MockFailureEvent.MockFailureEventNamespace);
                await mockFailureEventStream.OnNextAsync(new MockFailureEvent
                {
                    GrainId = this.GetPrimaryKey(),
                    ErrorCode = MockFailureEvent.MockErrorCode,
                    ErrorMessage = MockFailureEvent.MockErrorMessage
                });
            }
        }
    }
}