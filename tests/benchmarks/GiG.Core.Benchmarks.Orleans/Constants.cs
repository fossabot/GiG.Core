namespace GiG.Core.Benchmarks.Orleans
{
    public static class Constants
    {
        /// <summary>
        /// The name for the Orleans Simple Message Stream Provider.
        /// </summary>
        public const string SMSProviderName = "SMSProvider";

        /// <summary>
        /// The name for the Kafka Stream Provider.
        /// </summary>
        public const string KafkaProviderName = "KafkaProvider";

        /// <summary>
        /// The name for the Message Namespace
        /// </summary>
        public const string MessageNamespace = "Messages";

        /// <summary>
        /// The name for the in memory storage to be used for streams.
        /// </summary>
        public const string StreamsMemoryStorageName = "PubSubStore";

    }
}