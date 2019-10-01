using System;
using GiG.Core.Orleans.Sample.Contracts;
using Microsoft.Extensions.Logging;
using Orleans;
using System.Threading.Tasks;
using Orleans.Streams;

namespace GiG.Core.Orleans.Sample.Grains
{
    public class TransactionGrain : Grain, ITransactionGrain
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
            _balance += amount;

            await _stream.OnNextAsync(_balance);

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
            _balance -= amount;

            await _stream.OnNextAsync(_balance);
            
            return _balance;
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
