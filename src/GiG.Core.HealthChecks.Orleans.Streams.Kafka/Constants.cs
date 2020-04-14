namespace GiG.Core.HealthChecks.Orleans.Streams.Kafka
{
    internal static class Constants
    {
        /// <summary>
        /// The Default Health Check Name.
        /// </summary>
        public const string DefaultHealthCheckName = "KafkaOrleansStreams";

        /// <summary>
        /// The Default Kafka Topic Name.
        /// </summary>
        public const string DefaultTopicName = "Orleans_Streams_Health_Check";
    }
}