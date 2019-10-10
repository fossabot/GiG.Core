using GiG.Core.Orleans.Sample.Contracts.Models.Payment;
using Orleans;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Sample.Contracts
{
    public interface IPaymentTransactionGrain : IGrainWithGuidKey
    {
        Task<IEnumerable<PaymentTransaction>> GetAllAsync();
    }
}