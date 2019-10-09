using Refit;
using System.Net.Http;
using System.Threading.Tasks;

namespace GiG.Core.Http.Tests.Integration.Mocks
{
    public interface IMockRestClient
    {
        [Get("/api/mock")]
        Task<HttpResponseMessage> GetAsync();
    }
}