namespace GiG.Core.Orleans.Sample.Contracts.Models.Payment
{
    public class PaymentTransaction
    {
        public decimal Amount { get; set; }
        public PaymentTransactionType TransactionType { get; set; }
    }
}