namespace GiG.Core.Orleans.Sample.Grains.Contracts.Models.Wallet
{
    public class WalletTransaction
    {
        public decimal NewBalance { get; set; }

        public decimal Amount { get; set; }
        
        public WalletTransactionType TransactionType { get; set; }
    }
}