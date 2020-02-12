namespace GiG.Core.Orleans.Tests.Integration
{
    public static class Constants
    {
        /// <summary>
        /// The name for the Stream Provider.
        /// </summary>
        public const string StreamProviderName = "SMSProvider";

        /// <summary>
        /// The Activity Test stream namespace.
        /// </summary>
        public const string ActivityStreamNamespace = "ActivityTest";

        /// <summary>
        /// The name for the in memory storage to be used for streams.
        /// </summary>
        public const string StreamsMemoryStorageName = "PubSubStore";

        /// <summary>
        /// The storage provider to use for grain persistence.
        /// </summary>
        public const string StorageProviderName = "Default";
    }
}
