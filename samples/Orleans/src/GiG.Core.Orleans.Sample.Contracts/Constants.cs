﻿namespace GiG.Core.Orleans.Sample.Contracts
{
    /// <summary>
    /// Orleans Constants.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// The name for the Stream Provider.
        /// </summary>
        public const string StreamProviderName = "SMSProvider";
        
        /// <summary>
        /// The Wallet Balance Update stream namespace.
        /// </summary>
        public const string WalletTransactionsStreamNamespace = "WalletTransactions";
        
        /// <summary>
        /// The Wallet Balance Update stream namespace.
        /// </summary>
        public const string PaymentTransactionsStreamNamespace = "PaymentTransactions";
        
        /// <summary>
        /// The name for the in memory storage to be used for streams.
        /// </summary>
        public const string StreamsMemoryStorageName = "PubSubStore";

        /// <summary>
        /// The storage prodiver to use for grain persistance.
        /// </summary>
        public const string StorageProviderName = "SampleDb";
    }
}
