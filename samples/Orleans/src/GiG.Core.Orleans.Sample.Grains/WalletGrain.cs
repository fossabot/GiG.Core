using GiG.Core.Orleans.Sample.Contracts;
using GiG.Core.Orleans.Sample.Contracts.Models.Payment;
using GiG.Core.Orleans.Sample.Contracts.Models.Wallet;
using GiG.Core.Orleans.Sample.Hubs;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Providers;
using Orleans.Streams;
using System;
using System.Threading.Tasks;
using GiG.Core.Orleans.Sample.Hubs;
using SignalR.Orleans.Core;


namespace GiG.Core.Orleans.Sample.Grains
{
    [ImplicitStreamSubscription(Constants.PaymentTransactionsStreamNamespace)]
    [StorageProvider]
    public class WalletGrain : Grain<BalanceState>, IWalletGrain, IAsyncObserver<PaymentTransaction>
    {
        private IAsyncStream<WalletTransaction> _walletStream;
        private IAsyncStream<PaymentTransaction> _paymentStream;
              
        private readonly ILogger _logger;

        public WalletGrain(ILogger<WalletGrain> logger)
        {
            _logger = logger;            
        }
        
        public override async Task OnActivateAsync()
        {
            var streamProvider = GetStreamProvider(Constants.StreamProviderName);
            _paymentStream = streamProvider.GetStream<PaymentTransaction>(this.GetPrimaryKey(), Constants.PaymentTransactionsStreamNamespace);
            await _paymentStream.SubscribeOrResumeAsync(OnNextAsync);
            
            _walletStream = streamProvider.GetStream<WalletTransaction>(this.GetPrimaryKey(), Constants.WalletTransactionsStreamNamespace);
                       
            await base.OnActivateAsync();
        }

        /// <summary>
        /// Performs a Debit
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public async Task<decimal> DebitAsync(decimal amount)
        {
            _logger.LogInformation($"Debit {amount}");
            State.Amount += amount;

            var transactionModel = new WalletTransaction()
            {
                Amount = amount,
                NewBalance = State.Amount,
                TransactionType = WalletTransactionType.Debit
            };
            
            await _walletStream.OnNextAsync(transactionModel);
            await base.WriteStateAsync();

            return State.Amount;
        }

        /// <summary>
        /// Performs a Credit
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public async Task<decimal> CreditAsync(decimal amount)
        {
            //if (amount > State.Amount)
            //{
            //    throw new Exception("Credit Amount must be smaller or equal to the BalanceState.");
            //}
            
            _logger.LogInformation($"Credit {amount}");
            State.Amount -= amount;

            var transactionModel = new WalletTransaction()
            {
                Amount = amount,
                NewBalance = State.Amount,
                TransactionType = WalletTransactionType.Credit
            };
            
            await _walletStream.OnNextAsync(transactionModel);
            await base.WriteStateAsync();
            
            return State.Amount;
        }
        
        /// <summary>
        /// Get BalanceState
        /// </summary>
        /// <returns></returns>
        public Task<decimal> GetBalanceAsync()
        {
            return Task.FromResult(State.Amount);
        }

        public async Task OnNextAsync(PaymentTransaction item, StreamSequenceToken token = null)
        {
            switch (item.TransactionType)
            {
                case PaymentTransactionType.Deposit:
                    await DebitAsync(item.Amount);
                    break;
                case PaymentTransactionType.Withdrawal:
                    await CreditAsync(item.Amount);
                    break;
            }

            await GrainFactory.GetHub<NotificationsHub>().Group(this.GetPrimaryKey().ToString()).Send("BalanceChanged", State.Amount);
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