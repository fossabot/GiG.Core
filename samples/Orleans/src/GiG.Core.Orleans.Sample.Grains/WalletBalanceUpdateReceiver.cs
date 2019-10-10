using GiG.Core.Orleans.Sample.Grains.Contracts;
using GiG.Core.Orleans.Sample.Grains.Contracts.Models.Wallet;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Streams;
using System;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Sample.Grains
{
    [ImplicitStreamSubscription(Constants.WalletTransactionsStreamNamespace)]
    public class WalletBalanceUpdateReceiver : Grain, IBalanceUpdateReceiver, IAsyncObserver<WalletTransaction>
    {
        private IAsyncStream<WalletTransaction> _stream;
        private readonly ILogger _logger;

        public WalletBalanceUpdateReceiver(ILogger<WalletBalanceUpdateReceiver> logger)
        {
            _logger = logger;
        }
        
        public override async Task OnActivateAsync()
        {
            var streamProvider = GetStreamProvider(Constants.StreamProviderName);
            _stream = streamProvider.GetStream<WalletTransaction>(this.GetPrimaryKey(), Constants.WalletTransactionsStreamNamespace);
            await _stream.SubscribeOrResumeAsync(OnNextAsync);
            
            await base.OnActivateAsync();
        }
        
        public Task OnNextAsync(WalletTransaction item, StreamSequenceToken token = null)
        {
            _logger.LogInformation($"User {this.GetPrimaryKey()} Balance updated to {item.NewBalance}");
            
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