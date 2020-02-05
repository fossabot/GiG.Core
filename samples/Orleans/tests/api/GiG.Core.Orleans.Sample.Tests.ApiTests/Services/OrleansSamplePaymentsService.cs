using System;
using RestEase;
using System.Threading.Tasks;
using GiG.Core.Orleans.Sample.Tests.ApiTests.Contracts;

namespace GiG.Core.Orleans.Sample.Tests.ApiTests.Services
{
    internal class OrleansSamplePaymentsService: IOrleansSamplePaymentsService
    {
        private readonly IOrleansSamplePaymentsService _orleansSamplePaymentsService;

        public OrleansSamplePaymentsService()
        {
            var microServiceUrl = SampleApiTestSettings.ApiUrl() + "Payments/";
            _orleansSamplePaymentsService = RestClient.For<IOrleansSamplePaymentsService>(microServiceUrl);
        }

        public Guid PlayerId { get; set; }
        public string IPAddress { get; set; }

        public async Task<Response<decimal>> DepositAsync(TransactionRequest transactionRequest)
        {
            return await _orleansSamplePaymentsService.DepositAsync(transactionRequest);
        }

        public async Task<Response<decimal>> WithdrawAsync(TransactionRequest transactionRequest)
        {
            return await _orleansSamplePaymentsService.WithdrawAsync(transactionRequest);
        }

        public void SetHeaders(Guid playerId, string ipAddress)
        {
            _orleansSamplePaymentsService.PlayerId = playerId;
            _orleansSamplePaymentsService.IPAddress = string.IsNullOrEmpty(ipAddress) ? null : ipAddress;
        }
    }
}
