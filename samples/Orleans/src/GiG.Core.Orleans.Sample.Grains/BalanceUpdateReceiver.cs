using System;
using System.Threading.Tasks;
using GiG.Core.Orleans.Sample.Contracts;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Streams;

namespace GiG.Core.Orleans.Sample.Grains
{
    [ImplicitStreamSubscription(Constants.BalanceUpdateStreamNameSpace)]
    public class BalanceUpdateReceiver : Grain, IBalanceUpdateReceiver, IAsyncObserver<decimal>
    {
        private IAsyncStream<decimal> _stream;
        private readonly ILogger _logger;

        public override async Task OnActivateAsync()
        {
            var streamProvider = GetStreamProvider(Constants.StreamProviderName);
            _stream = streamProvider.GetStream<decimal>(this.GetPrimaryKey(), Constants.BalanceUpdateStreamNameSpace);

            await SubscribeAsync();
            
            await base.OnActivateAsync();
        }

        private async Task SubscribeAsync()
        {
            var subscriptionHandles = await _stream.GetAllSubscriptionHandles();

            if (subscriptionHandles.Count > 0)
            {
                foreach (var subscriptionHandle in subscriptionHandles)
                {
                    await subscriptionHandle.ResumeAsync(this.OnNextAsync);
                }
            }

            await _stream.SubscribeAsync(this.OnNextAsync);
        }

        public BalanceUpdateReceiver(ILogger<TransactionGrain> logger)
        {
            _logger = logger;
        }
        
        public Task OnNextAsync(decimal item, StreamSequenceToken token = null)
        {
            _logger.LogInformation($"User {this.GetPrimaryKey()} Balance updated to {item}");
            
            return Task.CompletedTask;
        }

        public Task OnCompletedAsync()
        {
            _logger.LogInformation("Stream is Complete");
            return Task.CompletedTask;
        }

        public Task OnErrorAsync(Exception ex)
        {
            _logger.LogError($"Stream has error: {ex.Message}");
            return Task.CompletedTask;
        }
    }
}