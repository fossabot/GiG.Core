using System;
using System.Collections.Generic;
using System.Text;

namespace GiG.Core.Orleans.Sample.Tests.ApiTests.Models
{
    public class PaymentTransaction
    {
        public decimal Amount { get; set; }
        public PaymentTransactionType TransactionType { get; set; }
    }
}