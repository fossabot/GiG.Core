using GiG.Core.Orleans.Sample.Contracts;
using Microsoft.Extensions.Logging;
using Orleans;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Sample.Grains
{
    public class TransactionGrain : Grain, ITransactionGrain
    {
        private readonly ILogger _logger;
        private decimal _balance = 0;

        public TransactionGrain(ILogger<TransactionGrain> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Performs a Deposit
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public Task<decimal> Deposit(decimal amount)
        {
            _logger.LogInformation($"Deposit {amount}");
            _balance += amount;

            return Task.FromResult(_balance);
        }

        /// <summary>
        /// Performs a Withdrawal
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public Task<decimal> Withdraw(decimal amount)
        {
            _logger.LogInformation($"Withdraw {amount}");
            _balance -= amount;
            
            return Task.FromResult(_balance);
        }
        
        /// <summary>
        /// Get Balance
        /// </summary>
        /// <returns></returns>
        public Task<decimal> GetBalance()
        {
            return Task.FromResult(_balance);
        }
    }
}
