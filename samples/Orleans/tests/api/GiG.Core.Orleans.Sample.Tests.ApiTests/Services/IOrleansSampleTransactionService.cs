using System.Threading.Tasks;
using RestEase;

namespace GiG.Core.Orleans.Sample.Tests.ApiTests.Services
{
    [AllowAnyStatusCode]
    public interface IOrleansSampleTransactionService
    {
        [Header("player-id")]
        string PlayerId { get; set; }

        [Header("X-Forwarded-For")]
        string IPAddress { get; set; }

        [Get("balance")]
        Task<Response<decimal>> GetBalanceAsync();

        [Post("deposit")]
        Task<Response<decimal>> DepositAsync([Body] TransactionRequest transactionRequest);

        [Post("withdraw")]
        Task<Response<decimal>> WithdrawAsync([Body] TransactionRequest transactionRequest);
    }
}
