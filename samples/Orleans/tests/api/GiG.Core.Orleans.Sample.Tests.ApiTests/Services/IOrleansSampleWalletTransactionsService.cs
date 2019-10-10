using System.Collections.Generic;
using System.Threading.Tasks;
using GiG.Core.Orleans.Sample.Grains.Contracts.Models.Wallet;
using RestEase;

namespace GiG.Core.Orleans.Sample.Tests.ApiTests.Services
{
    [AllowAnyStatusCode]
    public interface IOrleansSampleWalletTransactionsService : IOrleansSampleCommonService
    {
        [Get("WalletTransactions")]
        Task<Response<List<WalletTransaction>>> GetWalletTransactionsAsync();
    }
}
