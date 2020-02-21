using GiG.Core.Orleans.Streams.Abstractions;

namespace GiG.Core.Orleans.Sample.Contracts.Models.Wallet
{
    public class WalletTransaction : Message
    {
        public decimal NewBalance { get; set; }
        public decimal Amount { get; set; }
        public WalletTransactionType TransactionType { get; set; }
    }
}