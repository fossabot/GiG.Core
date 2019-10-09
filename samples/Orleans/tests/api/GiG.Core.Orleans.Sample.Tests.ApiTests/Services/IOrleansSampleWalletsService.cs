using System.Threading.Tasks;
using RestEase;
namespace GiG.Core.Orleans.Sample.Tests.ApiTests.Services
{
    [AllowAnyStatusCode]
    public interface IOrleansSampleWalletsService : IOrleansSampleCommonService
    {
        [Get("balance")]
        Task<Response<decimal>> GetBalanceAsync();
    }
}
