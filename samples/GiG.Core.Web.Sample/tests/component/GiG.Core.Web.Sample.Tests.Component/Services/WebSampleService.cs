
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RestEase;

namespace GiG.Core.Web.Sample.Tests.Component.Services
{
    public class WebSampleService
    {
        private const string MicroserviceUrl = "http://localhost:5000/transactions";
        ITransaction transactions;

        public WebSampleService() 
        {
            transactions = RestClient.For<ITransaction>(MicroserviceUrl);
        }

        public decimal GetBalance()
        {
            return transactions.GetBalanceAsync().Result;            
        }

        public Response<decimal> Deposit(TransactionRequest txRequest) 
        {
            return transactions.DepositAsync(txRequest).Result;
        }

        public Response<decimal> Withdraw(TransactionRequest txRequest)
        {
            return transactions.WithdrawAsync(txRequest).Result;
        }

        public decimal GetMinimumDepositLimit()
        {
            return transactions.GetMinimumDepositAmountAsync().Result;
        }
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
