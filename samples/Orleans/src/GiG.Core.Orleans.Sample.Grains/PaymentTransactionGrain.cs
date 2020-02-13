using GiG.Core.Orleans.Sample.Contracts;
using GiG.Core.Orleans.Sample.Contracts.Models.Payment;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Providers;
using Orleans.Runtime;
using Orleans.Streams;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Sample.Grains
{
    [ImplicitStreamSubscription(Constants.PaymentTransactionsStreamNamespace)]
    [StorageProvider(ProviderName = Constants.StorageProviderName)]
    public class PaymentTransactionGrain : Grain<List<PaymentTransaction>>, IPaymentTransactionGrain, IAsyncObserver<PaymentTransaction>
    {
        private readonly ILogger _logger;
        private IAsyncStream<PaymentTransaction> _stream;
      
        public PaymentTransactionGrain(ILogger<PaymentTransactionGrain> logger)
        {
            _logger = logger;
        }
        
        public override async Task OnActivateAsync()
        {
            var streamProvider = GetStreamProvider(Constants.StreamProviderName);

            _stream = streamProvider.GetStream<PaymentTransaction>(this.GetPrimaryKey(), Constants.PaymentTransactionsStreamNamespace);
            await _stream.SubscribeOrResumeAsync(OnNextAsync);
            
            await base.OnActivateAsync();
        }
        
        public Task OnNextAsync(PaymentTransaction item, StreamSequenceToken token = null)
        {
            State.Add(item);
            _logger.LogInformation("New {transactionType}. Amount: {amount}. CorrelationId : {activityId}", item.TransactionType, item.Amount, System.Diagnostics.Activity.Current?.RootId ?? string.Empty);

            return Task.CompletedTask;
        }

        public Task OnCompletedAsync()
        {
            _logger.LogInformation("Stream is Complete");

            return Task.CompletedTask;
        }

        public Task OnErrorAsync(Exception ex)
        {
            _logger.LogError(ex, "Stream has error: {message}", ex.Message);
            
            return Task.CompletedTask;
        }

        public Task<IEnumerable<PaymentTransaction>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<PaymentTransaction>>(State);
        }
    }
}