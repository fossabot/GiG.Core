using Refit;
using System.Net.Http;
using System.Threading.Tasks;

namespace GiG.Core.Http.Tests.Integration
{
    public interface IWalletRestClient
    {
        [Get("/wallets/balance")]
        Task<HttpResponseMessage> GetBalance();
    }
}