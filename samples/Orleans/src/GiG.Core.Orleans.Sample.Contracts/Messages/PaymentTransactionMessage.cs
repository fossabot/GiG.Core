using System;

namespace GiG.Core.Orleans.Sample.Grains.Contracts.Messages
{
    public class PaymentTransactionMessage
    {
        public Guid PlayerId { get; set; }
        
        public decimal Amount { get; set; }
        
        public PaymentTransactionType TransactionType { get; set; }
    }
}