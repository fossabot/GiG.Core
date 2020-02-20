using GiG.Core.Orleans.Streams.Abstractions;
using System.Collections.Generic;

namespace GiG.Core.Orleans.Sample.Contracts.Models.Payment
{
    public class PaymentTransaction : Message
    {
        public decimal Amount { get; set; }
        public PaymentTransactionType TransactionType { get; set; }
    }
}