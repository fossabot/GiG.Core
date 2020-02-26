using Orleans.Streams.Kafka.Config;

namespace GiG.Core.Orleans.Streams.Kafka.Configurations
{
    /// <summary>
    /// Kafka Settings for Orleans Streams.
    /// </summary>
    public class KafkaOptions
    {
        /// <summary>
        /// Default Section Name.
        /// </summary>
        public const string DefaultSectionName = "Orleans:Streams:Kafka";

        /// <summary>
        /// Broker List for Kafka.
        /// </summary>
        public string Brokers { get; set; } = "localhost:9092";

        /// <summary>
        /// Consumer Group Identifier.
        /// </summary>
        public string ConsumerGroupId { get; set; }
        
        /// <summary>
        /// The Sasl Username.
        /// </summary>
        public string SaslUsername { get; set; }

        /// <summary>
        /// The Sasl Password.
        /// </summary>
        public string SaslPassword { get; set; }

        /// <summary>
        /// SecurityProtocol enum value.
        /// </summary>
        public SecurityProtocol SecurityProtocol { get; set; } = SecurityProtocol.Plaintext;

        /// <summary>
        /// The SaslMechanism to use.
        /// </summary>
        public SaslMechanism SaslMechanism { get; set; } = SaslMechanism.Plain;
    }
}