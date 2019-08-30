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

        public decimal Deposit(decimal amount)
        {
            _logger.LogInformation($"Deposit {amount}");
            Balance += amount;

            return Balance;
        }

        public decimal Withdraw(decimal amount)
        {
            _logger.LogInformation($"Withdraw {amount}");
            Balance -= amount;
            
            return Balance;
        }
    }
}
