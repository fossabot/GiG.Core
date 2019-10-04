using System;
using System.Threading.Tasks;
using RestEase;
namespace GiG.Core.Orleans.Sample.Tests.ApiTests.Services
{
    [AllowAnyStatusCode]
    public interface IOrleansSampleWalletsService
    {
        [Header("player-id")]
        Guid PlayerId { get; set; }

        [Header("X-Forwarded-For")]
        string IPAddress { get; set; }

        [Get("balance")]
        Task<Response<decimal>> GetBalanceAsync();
    }
}
