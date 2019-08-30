using GiG.Core.Web.Sample.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace GiG.Core.Web.Sample.Tests.Component.Services
{
    public class WebSampleService
    {
        private const string MicroserviceUrl = "http://localhost:5000/transactions";


        public decimal GetBalance()
        {
            return 0;
        }

        public decimal Deposit(TransactionRequest txRequest) 
        {
            return 0;
        }

        public decimal Withdraw(TransactionRequest txRequest)
        {
            return 0;
        }

        public decimal GetMinimumDepositLimit()
        {
            return 0;
        }


        
    }
}
