using GiG.Core.Web.Sample.Contracts;
using Microsoft.Extensions.Logging;

namespace GiG.Core.Web.Sample.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ILogger _logger;

        public TransactionService(ILogger<TransactionService> logger)
        {
            _logger = logger;
        }

        public decimal Balance { get; private set; }

        /// <summary>
        /// Performs a Deposit
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public decimal Deposit(decimal amount)
        {
            _logger.LogInformation($"Deposit {amount}");
            Balance += amount;

            return Balance;
        }

        /// <summary>
        /// Performs a Withdrawal
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public decimal Withdraw(decimal amount)
        {
            _logger.LogInformation($"Withdraw {amount}");
            Balance -= amount;
            
            return Balance;
        }
    }
}
