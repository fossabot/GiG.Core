using Orleans;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Sample.Contracts
{
    public interface IWalletGrain : IGrainWithGuidKey
    {
        /// <summary>
        /// Performs a Debit
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        Task<decimal> Debit(decimal amount);

        /// <summary>
        /// Performs a Credit
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        Task<decimal> Credit(decimal amount);

        /// <summary>
        /// Get Balance
        /// </summary>
        /// <returns></returns>
        Task<decimal> GetBalance();
    }
}