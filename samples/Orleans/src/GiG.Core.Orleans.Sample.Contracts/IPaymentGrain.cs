using Orleans;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Sample.Contracts
{
    public interface IPaymentGrain : IGrainWithGuidKey
    {
        /// <summary>
        /// Performs a DepositAsync
        /// </summary>
        /// <param name="amount"></param>
        Task DepositAsync(decimal amount);

        /// <summary>
        /// Performs a Withdrawal
        /// </summary>
        /// <param name="amount"></param>
        Task WithdrawAsync(decimal amount);
    }
}