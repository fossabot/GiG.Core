using GiG.Core.Orleans.Sample.Grains.Contracts.Models.Wallet;
using Orleans;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Sample.Grains.Contracts
{
    public interface IWalletTransactionGrain : IGrainWithGuidKey
    {
        Task<IEnumerable<WalletTransaction>> GetAllAsync();
    }
}