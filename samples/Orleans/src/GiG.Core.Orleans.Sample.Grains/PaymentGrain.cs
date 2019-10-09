using GiG.Core.Orleans.Sample.Grains.Contracts;
using GiG.Core.Orleans.Sample.Grains.Contracts.Models.Payment;
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

        public PaymentGrain(ILogger<PaymentGrain> logger)
        {
            _logger = logger;
        }

        public override Task OnActivateAsync()
        {
            var streamProvider = GetStreamProvider(Constants.StreamProviderName);
            _stream = streamProvider.GetStream<PaymentTransaction>(this.GetPrimaryKey(),
                Constants.PaymentTransactionsStreamNamespace);

            return base.OnActivateAsync();
        }

        /// <summary>
        /// Performs a DepositAsync
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public async Task DepositAsync(decimal amount)
        {
            _logger.LogInformation($"DepositAsync {amount}");

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
        public async Task WithdrawAsync(decimal amount)
        {
            _logger.LogInformation($"WithdrawAsync {amount}");

            var transactionModel = new PaymentTransaction()
            {
                Amount = amount,
                TransactionType = PaymentTransactionType.Withdrawal
            };

            await _stream.OnNextAsync(transactionModel);
        }
    }
}