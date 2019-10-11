using System;

namespace GiG.Core.Orleans.Sample.Contracts.Messages
{
    public class PaymentTransactionRequested
    {
        public Guid PlayerId { get; set; }

        public decimal Amount { get; set; }

        public TransactionType TransactionType { get; set; }
    }
}