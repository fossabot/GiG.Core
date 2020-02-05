using GiG.Core.Orleans.Sample.Contracts;
using GiG.Core.Orleans.Sample.Contracts.Models.Payment;
using GiG.Core.Orleans.Sample.Contracts.Models.Wallet;
using GiG.Core.Orleans.Streams.Abstractions;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Providers;
using Orleans.Streams;
using System;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Sample.Grains
{
    [ImplicitStreamSubscription(Constants.PaymentTransactionsStreamNamespace)]
    [StorageProvider(ProviderName = Constants.StorageProviderName)]
    public class WalletGrain : Grain<BalanceState>, IWalletGrain, IAsyncObserver<PaymentTransaction>
    {
        private IAsyncStream<PaymentTransaction> _paymentStream;                     
        private readonly ILogger _logger;        
        private readonly IStreamFactory _streamFactory;

        private IStream<WalletTransaction> _stream;

        public WalletGrain(ILogger<WalletGrain> logger, IStreamFactory streamFactory)
        {
            _logger = logger;
            _streamFactory = streamFactory;
        }

        public override async Task OnActivateAsync()
        {
            var streamProvider = GetStreamProvider(Constants.StreamProviderName);
            _paymentStream = streamProvider.GetStream<PaymentTransaction>(this.GetPrimaryKey(), Constants.PaymentTransactionsStreamNamespace);
            await _paymentStream.SubscribeOrResumeAsync(OnNextAsync);

            _stream = _streamFactory.GetStream<WalletTransaction>(streamProvider, this.GetPrimaryKey(), Constants.WalletTransactionsStreamNamespace);
                                   
            await base.OnActivateAsync();
        }

        /// <summary>
        /// Performs a Debit
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public async Task<decimal> DebitAsync(decimal amount)
        {
            _logger.LogInformation("Debit {amount}", amount);
            State.Amount -= amount;

            var transactionModel = new WalletTransaction
            {
                Amount = amount,
                NewBalance = State.Amount,
                TransactionType = WalletTransactionType.Debit
            };

            await _stream.PublishAsync(transactionModel);
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
            
            _logger.LogInformation("Credit {amount}", amount);
            State.Amount += amount;

            var transactionModel = new WalletTransaction
            {
                Amount = amount,
                NewBalance = State.Amount,
                TransactionType = WalletTransactionType.Credit
            };

            await _stream.PublishAsync(transactionModel);
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
                    await CreditAsync(item.Amount);
                    break;
                case PaymentTransactionType.Withdrawal:
                    await DebitAsync(item.Amount);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // Publish new balance to SignalR hub.
            await GrainFactory.GetHub<INotificationsHub>().Group(this.GetPrimaryKey().ToString()).Send("BalanceChanged", State.Amount);
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
    }
}