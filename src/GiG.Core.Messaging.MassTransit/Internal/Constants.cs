namespace GiG.Core.Messaging.MassTransit.Internal
{
    /// <summary>
    /// Constants.
    /// </summary>
    internal static class Constants
    {
        /// <summary>
        /// The name of the header containing the Activity Id.
        /// </summary>
        public const string ActivityIdHeader = "Activity-Id";

        /// <summary>
        /// The Name of the Tracer.
        /// </summary>
        public const string TracerName = "RabbitMQTracer";

        /// <summary>
        /// The Prefix of the Tracing Span Operation for Publishing to RabbitMQ.
        /// </summary>
        public const string SpanPublishOperationNamePrefix = "RabbitMQPublish";

        /// <summary>
        /// The Name of the Activity started when publishing to RabbitMQ.
        /// </summary>
        public const string PublishActivityName = "RabbitMQPublish";

        /// <summary>
        /// The Prefix of the Tracing Span Operation for Consuming from RabbitMQ.
        /// </summary>
        public const string SpanConsumeOperationNamePrefix = "RabbitMQConsume";

        /// <summary>
        /// The Name of the Activity started when consuming RabbitMQ.
        /// </summary>
        public const string ConsumeActivityName = "RabbitMQConsume";
    }
}
