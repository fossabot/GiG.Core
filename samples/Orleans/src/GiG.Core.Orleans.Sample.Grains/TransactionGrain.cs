using GiG.Core.Orleans.Sample.Contracts;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Providers;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Sample.Grains
{
    /// <summary>
    /// State class for the Transaction Grain.
    /// </summary>
    public class TransactionState
    {
        /// <summary>
        /// The balance.
        /// </summary>
        public decimal Balance { get; set; } = 0;
    }

    [StorageProvider(ProviderName = Constants.InMemoryPersistanceName)]
    public class TransactionGrain : Grain<TransactionState>, ITransactionGrain
    {
        private readonly ILogger _logger;

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
            State.Balance += amount;

            base.WriteStateAsync();

            return Task.FromResult(State.Balance);
        }

        /// <summary>
        /// Performs a Withdrawal
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public Task<decimal> Withdraw(decimal amount)
        {
            _logger.LogInformation($"Withdraw {amount}");
            State.Balance -= amount;

            base.WriteStateAsync();

            return Task.FromResult(State.Balance);
        }
        
        /// <summary>
        /// Get Balance
        /// </summary>
        /// <returns></returns>
        public Task<decimal> GetBalance()
        {
            return Task.FromResult(State.Balance);
        }
    }
}
