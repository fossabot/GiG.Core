using System.Threading.Tasks;
using GiG.Core.Orleans.Sample.Tests.ApiTests.Contracts;
using RestEase;

namespace GiG.Core.Orleans.Sample.Tests.ApiTests.Services
{
    [AllowAnyStatusCode]
    public interface IOrleansSamplePaymentsService: IOrleansSampleCommonService
    {

        [Post("deposit")]
        Task<Response<decimal>> DepositAsync([Body] TransactionRequest transactionRequest);

        [Post("withdraw")]
        Task<Response<decimal>> WithdrawAsync([Body] TransactionRequest transactionRequest);
    }
}
