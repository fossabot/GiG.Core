using GiG.Core.Orleans.Sample.Web.Contracts;
using Refit;
using System.Net.Http;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Sample.Http.Contracts
{
    public interface IPaymentsClient
    {
        [Post("/deposit")]
        Task<HttpResponseMessage> DepositAsync([Header("player-id")] string playerId, [Body] TransactionRequest request);
    }
}