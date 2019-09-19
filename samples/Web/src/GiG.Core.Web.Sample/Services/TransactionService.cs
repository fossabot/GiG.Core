using GiG.Core.Context.Abstractions;
using GiG.Core.Web.Sample.Contracts;
using Microsoft.Extensions.Logging;

namespace GiG.Core.Web.Sample.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ILogger _logger;
        private readonly IRequestContextAccessor _requestContextAccessor;

        public TransactionService(ILogger<TransactionService> logger,
            IRequestContextAccessor requestContextAccessor)
        {
            _logger = logger;
            _requestContextAccessor = requestContextAccessor;
        }

        public decimal Balance { get; private set; }

        /// <summary>
        /// Performs a Deposit
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public decimal Deposit(decimal amount)
        {
            _logger.LogInformation($"Request IP Address {_requestContextAccessor.IPAddress.ToString()}");
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
