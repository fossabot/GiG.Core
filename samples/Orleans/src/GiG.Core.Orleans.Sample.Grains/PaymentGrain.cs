using GiG.Core.Orleans.Sample.Contracts;
using GiG.Core.Orleans.Sample.Contracts.Models.Payment;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Providers;
using Orleans.Streams;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Sample.Grains
{
    [StorageProvider]
    public class PaymentGrain : Grain, IPaymentGrain
    {
        private IAsyncStream<PaymentTransaction> _stream;

        private readonly ILogger _logger;

        public override Task OnActivateAsync()
        {
            var streamProvider = GetStreamProvider(Constants.StreamProviderName);
            _stream = streamProvider.GetStream<PaymentTransaction>(this.GetPrimaryKey(),
                Constants.PaymentTransactionsStreamNamespace);

            return base.OnActivateAsync();
        }

        public PaymentGrain(ILogger<PaymentGrain> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Performs a Deposit
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public async Task Deposit(decimal amount)
        {
            _logger.LogInformation($"Deposit {amount}");

            var transactionModel = new PaymentTransaction()
            {
                Amount = amount,
                TransactionType = PaymentTransactionType.Deposit
            };

            await _stream.OnNextAsync(transactionModel);
        }

        /// <summary>
        /// Performs a Withdrawal
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public async Task Withdraw(decimal amount)
        {
            _logger.LogInformation($"Withdraw {amount}");

            var transactionModel = new PaymentTransaction()
            {
                Amount = amount,
                TransactionType = PaymentTransactionType.Withdrawal
            };

            await _stream.OnNextAsync(transactionModel);
        }
    }
}