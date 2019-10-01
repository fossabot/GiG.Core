using Orleans;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Sample.Contracts
{
    public interface ITransactionGrain : IGrainWithGuidKey
    {
        /// <summary>
        /// Performs a Deposit
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        Task<decimal> Deposit(decimal amount);

        /// <summary>
        /// Performs a Withdrawal
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        Task<decimal> Withdraw(decimal amount);

        /// <summary>
        /// Get Balance
        /// </summary>
        /// <returns></returns>
        Task<decimal> GetBalance();
    }
}