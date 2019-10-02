using GiG.Core.Orleans.Sample.Contracts;
using GiG.Core.Orleans.Sample.Contracts.Models.Wallet;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Streams;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Sample.Grains
{
    [ImplicitStreamSubscription(Constants.WalletTransactionsStreamNamespace)]
    public class WalletTransactionGrain : Grain<List<WalletTransaction>>, IWalletTransactionGrain, IAsyncObserver<WalletTransaction>
    {
        private IAsyncStream<WalletTransaction> _stream;
        private readonly ILogger _logger;
      
        public WalletTransactionGrain(ILogger<WalletTransactionGrain> logger)
        {
            _logger = logger;
        }
        
        public override async Task OnActivateAsync()
        {
            var streamProvider = GetStreamProvider(Constants.StreamProviderName);
            _stream = streamProvider.GetStream<WalletTransaction>(this.GetPrimaryKey(), Constants.WalletTransactionsStreamNamespace);

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
        
        public Task OnNextAsync(WalletTransaction item, StreamSequenceToken token = null)
        {
            State.Add(item);
            
            _logger.LogInformation($"New {item.TransactionType.ToString()}. Amount: {item.Amount}. New Balance: {item.NewBalance}");

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

        public Task<IEnumerable<WalletTransaction>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<WalletTransaction>>(State);
        }
    }
}