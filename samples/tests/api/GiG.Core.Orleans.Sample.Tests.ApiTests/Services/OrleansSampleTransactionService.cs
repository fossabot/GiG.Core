using RestEase;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Sample.Tests.ApiTests.Services
{
    internal class OrleansSampleTransactionService
    {
        readonly IOrleansSampleTransactionService _orleansSampleTransactionService;

        public OrleansSampleTransactionService()
        {
            const string microServiceUrl = "http://localhost:7000/Transactions/";
            _orleansSampleTransactionService = RestClient.For<IOrleansSampleTransactionService>(microServiceUrl);
        }

        public async Task<Response<decimal>> GetBalanceAsync(string playerId, string ipAddress)
        {
            return await _orleansSampleTransactionService.GetBalanceAsync(playerId, ipAddress);
        }

        public async Task<Response<decimal>> DepositAsync(string playerId, string ipAddress, TransactionRequest transactionRequest)
        {
            return  await _orleansSampleTransactionService.DepositAsync(playerId, ipAddress, transactionRequest);
        }

        public async Task<Response<decimal>> WithdrawAsync(string playerId, string ipAddress, TransactionRequest transactionRequest)
        {
            return await _orleansSampleTransactionService.WithdrawAsync(playerId, ipAddress, transactionRequest);
        }
    }

}
