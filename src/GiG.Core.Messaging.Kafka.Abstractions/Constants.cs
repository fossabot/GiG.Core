namespace GiG.Core.Messaging.Kafka.Abstractions
{
    /// <summary>
    /// Constants.
    /// </summary>
    public class Constants
    {
        /// <summary>
        /// Header name for the Message type.
        /// </summary>
        public const string MessageTypeHeaderName = "message_type";
        
        /// <summary>
        /// Header name for the Message ID.
        /// </summary>
        public const string MessageIdHeaderName = "message_id";
        
        /// <summary>
        /// Header name for the Message ID.
        /// </summary>
        public const string CorrelationIdHeaderName = "correlation_id";
        
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