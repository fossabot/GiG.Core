using GiG.Core.DistributedTracing.Abstractions;
using GiG.Core.MultiTenant.Abstractions;
using GiG.Core.Orleans.Streams.Abstractions;
using GiG.Core.Orleans.Tests.Integration.Contracts;
using GiG.Core.Orleans.Tests.Integration.Mocks;
using Orleans;
using Orleans.Runtime;
using Orleans.Streams;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Tests.Integration.Grains
{
    [ImplicitStreamSubscription(Constants.ActivityStreamNamespace)]
    public class ActivityTestGrain : Grain, IActivityTestGrain, IAsyncObserver<MockMessage>
    {
        private readonly IActivityContextAccessor _activityContextAccessor;
        private readonly ITenantAccessor _tenantAccessor;
        private readonly IStreamFactory _streamFactory;
        private readonly IPersistentState<ActivityState> _state;

        private IStream<MockMessage> _incomingStream;

        public ActivityTestGrain([PersistentState(Constants.StorageProviderName, Constants.StorageProviderName)] IPersistentState<ActivityState> state,
            IActivityContextAccessor activityContextAccessor, ITenantAccessor tenantAccessor, IStreamFactory streamFactory =null)
        {
            _activityContextAccessor = activityContextAccessor;
            _tenantAccessor = tenantAccessor;
            _streamFactory = streamFactory;
            _state = state;
        }

        public override async Task OnActivateAsync()
        {
            //this will subscribe to kyc request events on the underlying stream provider
            var streamProvider = GetStreamProvider(Constants.StreamProviderName);
            _incomingStream = _streamFactory.GetStream<MockMessage>(streamProvider, this.GetPrimaryKey(), Constants.ActivityStreamNamespace);
            await _incomingStream.SubscribeAsync(this);

            await base.OnActivateAsync();
        }

        public async Task OnNextAsync(MockMessage documentVerificationRequestEvent, StreamSequenceToken sequenceToken)
        {
            _state.State = new ActivityState
            {
                Activity = new ActivityResponse
                {
                    TraceId = _activityContextAccessor.TraceId,
                    ParentId = _activityContextAccessor.ParentId,
                    RootId = _activityContextAccessor.CorrelationId,
                    Baggage = GetBaggage(),
                    TenantId = GetTenant()
                }
            };
               
            await _state.WriteStateAsync();
        }

        public Task<ActivityResponse> GetActivityAsync()
        {
            var activity = new ActivityResponse
            {
                TraceId = _activityContextAccessor.TraceId,
                ParentId = _activityContextAccessor.ParentId,
                RootId = _activityContextAccessor.CorrelationId,
                Baggage = GetBaggage(),
                TenantId = GetTenant()
            };
            return Task.FromResult(activity);
        }

        public Task<ActivityResponse> GetStreamActivityAsync()
        {
            return Task.FromResult(_state.State.Activity);
        }

        public Task OnCompletedAsync()
        {
            return Task.CompletedTask;
        }

        public Task OnErrorAsync(Exception ex)
        {
            return Task.CompletedTask;
        }

        private string GetBaggage()
        {
            return _activityContextAccessor.Baggage.FirstOrDefault(x => x.Key == "BaggageTest").Value;
        }
        
        private string GetTenant()
        {
            return string.Join(';', _tenantAccessor.Values);
        }
    }
}