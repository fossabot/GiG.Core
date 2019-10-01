using RestEase;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Sample.Tests.ApiTests.Services
{
    internal class OrleansSampleTransactionService: IOrleansSampleTransactionService
    {
        readonly IOrleansSampleTransactionService _orleansSampleTransactionService;

        public OrleansSampleTransactionService()
        {
            const string microServiceUrl = "http://localhost:7000/Transactions/";
            _orleansSampleTransactionService = RestClient.For<IOrleansSampleTransactionService>(microServiceUrl);
        }

        public string PlayerId { get; set; }
        public string IPAddress { get; set; }

        public async Task<Response<decimal>> GetBalanceAsync()
        {
            return await _orleansSampleTransactionService.GetBalanceAsync();
        }

        public async Task<Response<decimal>> DepositAsync(TransactionRequest transactionRequest)
        {
            return  await _orleansSampleTransactionService.DepositAsync(transactionRequest);
        }

        public async Task<Response<decimal>> WithdrawAsync(TransactionRequest transactionRequest)
        {
            return await _orleansSampleTransactionService.WithdrawAsync(transactionRequest);
        }

        public void SetHeaders(string playerId, string ipAddress)
        {
            _orleansSampleTransactionService.PlayerId = playerId;
            _orleansSampleTransactionService.IPAddress = ipAddress.Equals("") ? null : ipAddress;
        }
    }

}