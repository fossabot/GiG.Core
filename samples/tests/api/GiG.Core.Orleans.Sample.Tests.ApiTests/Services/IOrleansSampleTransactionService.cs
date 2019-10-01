using System.Threading;
using System.Threading.Tasks;
using RestEase;

namespace GiG.Core.Orleans.Sample.Tests.ApiTests.Services
{
    [AllowAnyStatusCode]
    public interface IOrleansSampleTransactionService
    {

        [Get("balance")]
        Task<Response<decimal>> GetBalanceAsync([Header("player-id")] string playerId, [Header("X-Forwarded-For")] string ipAddress, CancellationToken token = default);

        [Post("deposit")]
        Task<Response<decimal>> DepositAsync([Header("player-id")] string playerId, [Header("X-Forwarded-For")] string ipAddress, [Body] TransactionRequest transactionRequest);

        [Post("withdraw")]
        Task<Response<decimal>> WithdrawAsync([Header("player-id")] string playerId, [Header("X-Forwarded-For")] string ipAddress, [Body] TransactionRequest transactionRequest);
    }
}
