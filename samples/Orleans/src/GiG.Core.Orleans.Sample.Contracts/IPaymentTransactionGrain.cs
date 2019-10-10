using GiG.Core.Orleans.Sample.Grains.Contracts.Models.Payment;
using Orleans;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Sample.Grains.Contracts
{
    public interface IPaymentTransactionGrain : IGrainWithGuidKey
    {
        Task<IEnumerable<PaymentTransaction>> GetAllAsync();
    }
}