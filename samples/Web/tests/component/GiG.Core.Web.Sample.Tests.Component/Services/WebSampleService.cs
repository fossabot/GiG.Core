using RestEase;
using System.Threading.Tasks;

namespace GiG.Core.Web.Sample.Tests.Component.Services
{
    public class WebSampleService
    {
        private const string MicroserviceUrl = "http://localhost:5000/v2/transactions";
        private readonly ITransaction _transactions;

        public WebSampleService() => _transactions = RestClient.For<ITransaction>(MicroserviceUrl);

        public decimal GetBalance() => _transactions.GetBalanceAsync().Result;

        public Response<decimal> Deposit(TransactionRequest txRequest) => _transactions.DepositAsync(txRequest).Result;

        public Response<decimal> Withdraw(TransactionRequest txRequest) => _transactions.WithdrawAsync(txRequest).Result;

        public decimal GetMinimumDepositLimit() => _transactions.GetMinimumDepositAmountAsync().Result;
    }

    public class TransactionRequest
    {
        public decimal Amount { get; set; }
    }

    public interface ITransaction
    {
        [Get("balance")]
        Task<decimal> GetBalanceAsync();

        [Get("min-dep-amt")]
        Task<decimal> GetMinimumDepositAmountAsync();

        [Post("deposit")]
        [AllowAnyStatusCode]
        Task<Response<decimal>> DepositAsync([Body] TransactionRequest transactionRequest);

        [Post("withdraw")]
        [AllowAnyStatusCode]
        Task<Response<decimal>> WithdrawAsync([Body] TransactionRequest transactionRequest);
    }
}
