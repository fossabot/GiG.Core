using System.Threading.Tasks;
using Orleans;

namespace GiG.Core.Orleans.Sample.Contracts
{
    public interface IPaymentGrain : IGrainWithGuidKey
    {
        /// <summary>
        /// Performs a Deposit
        /// </summary>
        /// <param name="amount"></param>
        Task Deposit(decimal amount);

        /// <summary>
        /// Performs a Withdrawal
        /// </summary>
        /// <param name="amount"></param>
        Task Withdraw(decimal amount);
    }
}