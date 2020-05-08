namespace GiG.Core.Messaging.Kafka.Internal
{
    /// <summary>
    /// Constants.
    /// </summary>
    internal static class Constants
    {
        /// <summary>
        /// The Name of the Tracer.
        /// </summary>
        public const string TracerName = "KafkaTracer";

        /// <summary>
        /// The Prefix of the Tracing Span Operation for Publishing to a Kafka.
        /// </summary>
        public const string SpanPublishOperationNamePrefix = "KafkaPublish";

        /// <summary>
        /// The Name of the Activity started when publishing to Kafka.
        /// </summary>
        public const string PublishActivityName = "KafkaPublish";

        /// <summary>
        /// The Prefix of the Tracing Span Operation for Consuming Kafka Messages.
        /// </summary>
        public const string SpanConsumeOperationNamePrefix = "KafkaConsume";
        
        /// <summary>
        /// The Prefix of the Tracing Span Operation for Consuming Kafka Messages.
        /// </summary>
        public const string SpanConsumeCommitOperationNamePrefix = "KafkaConsumeCommit";

        /// <summary>
        /// The Name of the Activity started when consuming Kafka Messages.
        /// </summary>
        public const string ConsumeActivityName = "KafkaConsume";
    }
}