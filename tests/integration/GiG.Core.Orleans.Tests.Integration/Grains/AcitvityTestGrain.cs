using GiG.Core.DistributedTracing.Abstractions;
using GiG.Core.Orleans.Tests.Integration.Contracts;
using GiG.Core.Orleans.Tests.Integration.Mocks;
using Orleans;
using Orleans.Runtime;
using Orleans.Streams;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Tests.Integration.Grains
{
    [ImplicitStreamSubscription(Constants.ActivityStreamNamespace)]

    public class AcitvityTestGrain : Grain, IActivityTestGrain
    {
        private readonly IActivityContextAccessor _activityContextAccessor;
        private readonly IPersistentState<ActivityState> _state;

        private IAsyncStream<MockMessage> _incomingStream;

        public AcitvityTestGrain([PersistentState(Constants.StorageProviderName, Constants.StorageProviderName)] IPersistentState<ActivityState> state,
            IActivityContextAccessor activityContextAccessor)
        {
            _activityContextAccessor = activityContextAccessor;
            _state = state;
        }

        public override async Task OnActivateAsync()
        {
            //this will subscribe to kyc request events on the underlying stream provider
            var streamProvider = GetStreamProvider(Constants.StreamProviderName);
            _incomingStream = streamProvider.GetStream<MockMessage>(this.GetPrimaryKey(), Constants.ActivityStreamNamespace);
            await _incomingStream.SubscribeAsync(OnNextAsync);

            await base.OnActivateAsync();
        }

        private async Task OnNextAsync(MockMessage documentVerificationRequestEvent, StreamSequenceToken sequenceToken = null)
        {
            _state.State = new ActivityState
            {
                Activity = new ActivityResponse
                {
                    TraceId = _activityContextAccessor.TraceId,
                    ParentId = _activityContextAccessor.ParentId
                }
            };
               
            await _state.WriteStateAsync();
        }

        public Task<ActivityResponse> GetActivityAsync()
        {
            var activity = new ActivityResponse
            {
                TraceId = _activityContextAccessor.TraceId,
                ParentId = _activityContextAccessor.ParentId
            };
            return Task.FromResult(activity);
        }

        public Task<ActivityResponse> GetStreamActivityAsync()
        {
            return Task.FromResult(_state.State.Activity);
        }
    }
}