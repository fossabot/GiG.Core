namespace GiG.Core.Orleans.Streams.Kafka.Abstractions
{
    /// <summary>
    /// Kafka topic options for Orleans Streams.
    /// </summary>
    public class KafkaTopicOptions
    {
        /// <summary>
        /// The configuration default section name.
        /// </summary>
        public const string DefaultSectionName = "Orleans:Streams:Kafka:Topic";
        
        /// <summary>
        /// If enabled, topics will be created on application startup.
        /// </summary>
        public bool IsTopicCreationEnabled  { get; set; } = true;

        /// <summary>
        /// Number of Partitions.
        /// </summary>
        public int Partitions  { get; set; } = 1;

        /// <summary>
        /// Number of replicas of a topic in a Kafka cluster.
        /// </summary>
        public short ReplicationFactor  { get; set; } = 1;

        /// <summary>
        /// Retention Period in Milliseconds.
        /// </summary>
        public ulong? RetentionPeriodInMs { get; set; }
    }
}