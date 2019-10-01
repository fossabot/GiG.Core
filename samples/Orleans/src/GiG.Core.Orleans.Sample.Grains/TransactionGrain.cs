using System;
using GiG.Core.Orleans.Sample.Contracts;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Providers;
using System.Threading.Tasks;
using Orleans.Streams;

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
        private IAsyncStream<decimal> _stream;
        
        private readonly ILogger _logger;
        private decimal _balance = 0;

        public override Task OnActivateAsync()
        {
            var streamProvider = GetStreamProvider(Constants.StreamProviderName);
            _stream = streamProvider.GetStream<decimal>(this.GetPrimaryKey(), Constants.BalanceUpdateStreamNameSpace);
            
            return base.OnActivateAsync();
        }

        public TransactionGrain(ILogger<TransactionGrain> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Performs a Deposit
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public async Task<decimal> Deposit(decimal amount)
        {
            _logger.LogInformation($"Deposit {amount}");
            State.Balance += amount;

            await _stream.OnNextAsync(_balance);
            await base.WriteStateAsync();

            return _balance;
        }

        /// <summary>
        /// Performs a Withdrawal
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public async Task<decimal> Withdraw(decimal amount)
        {
            _logger.LogInformation($"Withdraw {amount}");
            State.Balance -= amount;

            await _stream.OnNextAsync(_balance);
            await base.WriteStateAsync();
            
            return _balance;
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
