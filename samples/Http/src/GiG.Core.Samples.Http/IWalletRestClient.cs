using Refit;
using System.Net.Http;
using System.Threading.Tasks;

namespace GiG.Core.Samples.Http
{
    public interface IWalletRestClient
    {
        [Get("/wallets/balance")]
        Task<HttpResponseMessage> GetBalance();
    }
}