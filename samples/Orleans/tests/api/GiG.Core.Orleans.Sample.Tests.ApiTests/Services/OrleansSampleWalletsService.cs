using System;
using System.Threading.Tasks;
using RestEase;

namespace GiG.Core.Orleans.Sample.Tests.ApiTests.Services
{
    public class OrleansSampleWalletsService: IOrleansSampleWalletsService
    {
        private readonly IOrleansSampleWalletsService _orleansSampleWalletsService;

        public OrleansSampleWalletsService()
        {
            string microServiceUrl = SampleApiTestSettings.ApiUrl() + "transactions/Wallets/";
            _orleansSampleWalletsService = RestClient.For<IOrleansSampleWalletsService>(microServiceUrl);
        }

        public Guid PlayerId { get; set; }
        public string IPAddress { get; set; }

        public async Task<Response<decimal>> GetBalanceAsync()
        {
            return await _orleansSampleWalletsService.GetBalanceAsync();
        }

        public void SetHeaders(Guid playerId, string ipAddress)
        {
            _orleansSampleWalletsService.PlayerId = playerId;
            _orleansSampleWalletsService.IPAddress = string.IsNullOrEmpty(ipAddress) ? null : ipAddress;
        }
    }
}