﻿using GiG.Core.Orleans.Sample.Contracts;
using GiG.Core.Orleans.Sample.Contracts.Models.Payment;
using GiG.Core.Orleans.Sample.Contracts.Models.Wallet;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Providers;
using Orleans.Streams;
using System;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Sample.Grains
{
    /// <summary>
    /// State class for the Transaction Grain.
    /// </summary>
    public class Balance
    {
        /// <summary>
        /// The balance.
        /// </summary>
        public decimal Amount { get; set; } = 0;
    }

    [ImplicitStreamSubscription(Constants.PaymentTransactionsStreamNamespace)]
    [StorageProvider]
    public class WalletGrain : Grain<Balance>, IWalletGrain, IAsyncObserver<PaymentTransaction>
    {
        private IAsyncStream<WalletTransaction> _walletStream;
        private IAsyncStream<PaymentTransaction> _paymentStream;
        
        private readonly ILogger _logger;
        private decimal _balance = 0;

        public override async Task OnActivateAsync()
        {
            var streamProvider = GetStreamProvider(Constants.StreamProviderName);
            _paymentStream = streamProvider.GetStream<PaymentTransaction>(this.GetPrimaryKey(), Constants.PaymentTransactionsStreamNamespace);
            await SubscribeAsync();
            
            _walletStream = streamProvider.GetStream<WalletTransaction>(this.GetPrimaryKey(), Constants.WalletTransactionsStreamNamespace);
            
            await base.OnActivateAsync();
        }
        
        private async Task SubscribeAsync()
        {
            var subscriptionHandles = await _paymentStream.GetAllSubscriptionHandles();

            if (subscriptionHandles.Count > 0)
            {
                foreach (var subscriptionHandle in subscriptionHandles)
                {
                    await subscriptionHandle.ResumeAsync(this.OnNextAsync);
                }
            }

            await _paymentStream.SubscribeAsync(this.OnNextAsync);
        }
        

        public WalletGrain(ILogger<WalletGrain> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Performs a Debit
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public async Task<decimal> Debit(decimal amount)
        {
            _logger.LogInformation($"Debit {amount}");
            State.Amount += amount;

            var transactionModel = new WalletTransaction()
            {
                Amount = amount,
                NewBalance = State.Amount,
                TransactionType = WalletTransactionType.Deposit
            };
            
            await _walletStream.OnNextAsync(transactionModel);
            await base.WriteStateAsync();

            return _balance;
        }

        /// <summary>
        /// Performs a Credit
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public async Task<decimal> Credit(decimal amount)
        {
            if (amount > State.Amount)
            {
                throw new Exception("Credit Amount must be smaller or equal to the Balance.");
            }
            
            _logger.LogInformation($"Credit {amount}");
            State.Amount -= amount;

            var transactionModel = new WalletTransaction()
            {
                Amount = amount,
                NewBalance = State.Amount,
                TransactionType = WalletTransactionType.Withdrawal
            };
            
            await _walletStream.OnNextAsync(transactionModel);
            await base.WriteStateAsync();
            
            return _balance;
        }
        
        /// <summary>
        /// Get Balance
        /// </summary>
        /// <returns></returns>
        public Task<decimal> GetBalance()
        {
            return Task.FromResult(State.Amount);
        }

        public async Task OnNextAsync(PaymentTransaction item, StreamSequenceToken token = null)
        {
            switch (item.TransactionType)
            {
                case PaymentTransactionType.Deposit:
                    await Debit(item.Amount);
                    break;
                case PaymentTransactionType.Withdrawal:
                    await Credit(item.Amount);
                    break;
            }
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
