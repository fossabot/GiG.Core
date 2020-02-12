using GiG.Core.DistributedTracing.Abstractions;
using GiG.Core.Orleans.Sample.Contracts;
using GiG.Core.Orleans.Sample.Contracts.Models.Wallet;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Providers;
using Orleans.Streams;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Constants = GiG.Core.Orleans.Sample.Contracts.Constants;

namespace GiG.Core.Orleans.Sample.Grains
{
    [ImplicitStreamSubscription(Constants.WalletTransactionsStreamNamespace)]
    [StorageProvider(ProviderName = Constants.StorageProviderName)]

    public class WalletTransactionGrain : Grain<List<WalletTransaction>>, IWalletTransactionGrain, IAsyncObserver<WalletTransaction>
    {
        private IAsyncStream<WalletTransaction> _stream;
        private readonly ILogger _logger;
        private readonly IActivityContextAccessor _activityContextAccessor;
      
        public WalletTransactionGrain(ILogger<WalletTransactionGrain> logger, IActivityContextAccessor activityContextAccessor)
        {
            _logger = logger;
            _activityContextAccessor = activityContextAccessor;
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

            _logger.LogInformation("Activity Id {correlationId}", _activityContextAccessor.TraceId);
            _logger.LogInformation("New {transactionType}. Amount: {amount}. New Balance: {newBalance}", item.TransactionType.ToString(), item.Amount, item.NewBalance);

            return Task.CompletedTask;
        }

        public Task OnCompletedAsync()
        {
            _logger.LogInformation("Stream is Complete");
            
            return Task.CompletedTask;
        }

        public Task OnErrorAsync(Exception ex)
        {
            _logger.LogError(ex, "Stream has error: {errorMessage}", ex.Message);

            return Task.CompletedTask;
        }

        public Task<IEnumerable<WalletTransaction>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<WalletTransaction>>(State);
        }
    }
}