using Orleans;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Sample.Grains.Contracts
{
    public interface IWalletGrain : IGrainWithGuidKey
    {
        /// <summary>
        /// Performs a Debit
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        Task<decimal> DebitAsync(decimal amount);

        /// <summary>
        /// Performs a Credit
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        Task<decimal> CreditAsync(decimal amount);

        /// <summary>
        /// Get Balance
        /// </summary>
        /// <returns></returns>
        Task<decimal> GetBalanceAsync();
    }
}