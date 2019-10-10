using System;

namespace GiG.Core.Orleans.Sample.Grains.Contracts.Messages
{
    public class PaymentTransactionMessage
    {
        public Guid PlayerId { get; set; }

        public decimal Amount { get; set; }

        public TransactionType TransactionType { get; set; }
    }
}