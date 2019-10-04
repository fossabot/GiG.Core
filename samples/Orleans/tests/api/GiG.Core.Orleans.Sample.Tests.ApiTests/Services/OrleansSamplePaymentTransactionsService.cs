using System;
using System.Collections.Generic;
using RestEase;
using System.Threading.Tasks;
using GiG.Core.Orleans.Sample.Tests.ApiTests.Models;

namespace GiG.Core.Orleans.Sample.Tests.ApiTests.Services
{
    internal class OrleansSamplePaymentTransactionsService: IOrleansSamplePaymentTransactionsService
    {
        readonly IOrleansSamplePaymentTransactionsService _orleansSamplePaymentTransactionsService;

        public OrleansSamplePaymentTransactionsService()
        {
            string microServiceUrl = SampleApiTestSettings.BaseUrl() + "transactions/";
            _orleansSamplePaymentTransactionsService = RestClient.For<IOrleansSamplePaymentTransactionsService>(microServiceUrl);
        }

        public Guid PlayerId { get; set; }
        public string IPAddress { get; set; }

        public async Task<Response<List<PaymentTransaction>>> GetPaymentTransactionsAsync()
        {
            return await _orleansSamplePaymentTransactionsService.GetPaymentTransactionsAsync();
        }

        public void SetHeaders(Guid playerId, string ipAddress)
        {
            _orleansSamplePaymentTransactionsService.PlayerId = playerId;
            _orleansSamplePaymentTransactionsService.IPAddress = string.IsNullOrEmpty(ipAddress) ? null : ipAddress;
        }
    }

}