using System.Collections.Generic;
using System.Threading.Tasks;
using GiG.Core.Orleans.Sample.Contracts.Models.Wallet;
using RestEase;

namespace GiG.Core.Orleans.Sample.Tests.ApiTests.Services
{
    [AllowAnyStatusCode]
    public interface IOrleansSampleWalletTransactionsService : IOrleansSampleCommonService
    {
        [Get]
        Task<Response<List<WalletTransaction>>> GetWalletTransactionsAsync();
    }
}
