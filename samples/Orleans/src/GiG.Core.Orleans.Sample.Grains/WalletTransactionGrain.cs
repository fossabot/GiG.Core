using GiG.Core.Orleans.Sample.Contracts;
using GiG.Core.Orleans.Sample.Contracts.Models.Wallet;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Providers;
using Orleans.Streams;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Sample.Grains
{
    [ImplicitStreamSubscription(Constants.WalletTransactionsStreamNamespace)]
    [StorageProvider(ProviderName = Constants.StorageProviderName)]

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
            await _stream.SubscribeOrResumeAsync(OnNextAsync);
            
            await base.OnActivateAsync();
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