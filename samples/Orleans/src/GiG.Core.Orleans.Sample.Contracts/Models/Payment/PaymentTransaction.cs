namespace GiG.Core.Orleans.Sample.Grains.Contracts.Models.Payment
{
    public class PaymentTransaction
    {
        public decimal Amount { get; set; }

        public PaymentTransactionType TransactionType { get; set; }
    }
}