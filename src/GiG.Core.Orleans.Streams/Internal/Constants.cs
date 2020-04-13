namespace GiG.Core.Orleans.Streams.Internal
{
    /// <summary>
    /// Constants.
    /// </summary>
    internal static class Constants
    {
        /// <summary>
        /// The Name of the Tracer.
        /// </summary>
        public const string TracerName = "StreamTracer";

        /// <summary>
        /// The Prefix of the TRacing Span Operation for Publishing to a Stream.
        /// </summary>
        public const string SpanPublishOperationNamePrefix = "StreamPublish";

        /// <summary>
        /// The Name of the Activity started when publishing to Streams.
        /// </summary>
        public const string PublishActivityName = "StreamPublish";

        /// <summary>
        /// The Prefix of the Tracing Span Operation for Consuming from Stream.
        /// </summary>
        public const string SpanConsumeOperationNamePrefix = "StreamConsume";

        /// <summary>
        /// The Name of the Activity started when consuming Streams.
        /// </summary>
        public const string ConsumeActivityName = "StreamConsume";
    }
}
