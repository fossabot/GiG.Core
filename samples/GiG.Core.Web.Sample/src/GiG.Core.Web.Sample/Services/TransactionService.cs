using GiG.Core.Web.Sample.Contracts;
using Microsoft.Extensions.Logging;

namespace GiG.Core.Web.Sample.Services
{
    public class TransactionService : ITransactionService
    {
        private decimal _playerBalance {get;set;}
        private ILogger _logger { get; set; }

        public TransactionService(ILogger<TransactionService> logger)
        {
            _logger = logger;
        }

        public decimal GetBalance()
        {
            return _playerBalance;
        }

        public decimal Deposit(decimal amount)
        {
            _logger.LogInformation($"Deposit {amount}");
            _playerBalance += amount;
            return GetBalance();
        }

        public decimal Withdraw(decimal amount)
        {
            _logger.LogInformation($"Withdraw {amount}");
            _playerBalance -= amount;
            return GetBalance();
        }

    }
}
