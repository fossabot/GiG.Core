using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GiG.Core.Orleans.Sample.Contracts.Models.Wallet;
using RestEase;

namespace GiG.Core.Orleans.Sample.Tests.ApiTests.Services
{
    public class OrleansSampleWalletTransactionsService: IOrleansSampleWalletTransactionsService
    {
        private readonly IOrleansSampleWalletTransactionsService _orleansSampleWalletTransactionsService;

        public OrleansSampleWalletTransactionsService()
        {
            string microServiceUrl = SampleApiTestSettings.ApiUrl() + "transactions/";
            _orleansSampleWalletTransactionsService = RestClient.For<IOrleansSampleWalletTransactionsService>(microServiceUrl);
        }

        public Guid PlayerId { get; set; }
        public string IPAddress { get; set; }

        public async Task<Response<List<WalletTransaction>>> GetWalletTransactionsAsync()
        {
            return await _orleansSampleWalletTransactionsService.GetWalletTransactionsAsync();
        }

        public void SetHeaders(Guid playerId, string ipAddress)
        {
            _orleansSampleWalletTransactionsService.PlayerId = playerId;
            _orleansSampleWalletTransactionsService.IPAddress = string.IsNullOrEmpty(ipAddress) ? null : ipAddress;
        }
    }
}
