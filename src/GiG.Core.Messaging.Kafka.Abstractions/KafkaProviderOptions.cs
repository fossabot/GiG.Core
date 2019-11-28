using Confluent.Kafka;
using System.Collections.Generic;

namespace GiG.Core.Messaging.Kafka.Abstractions
{
    /// <summary>
    /// Kafka provider options.
    /// </summary>
    public class KafkaProviderOptions
    {
        /// <summary>
        /// The Default config section name.  
        /// </summary>
        public const string DefaultSectionName = "EventProvider";

        /// <summary>
        /// The Avro schema registry URL.
        /// </summary>
        public string SchemaRegistry { get; set; } = "localhost:8081";

        /// <summary>
        /// List of Bootstrap servers.
        /// </summary>
        public string BootstrapServers { get; set; } = "localhost:9092";

        /// <summary>
        /// The group id.
        /// </summary>
        public string GroupId { get; set; } = "default-group";

        /// <summary>
        /// The topic name.
        /// </summary>
        public string Topic { get; set; } = "default-topic";

        /// <summary>
        /// Message timeout in milliseconds.
        /// </summary>
        public int MessageTimeoutMs { get; set; } = 25000;

        /// <summary>
        /// AutoOffsetReset enum value.
        /// </summary>
        public AutoOffsetReset AutoOffsetReset { get; set; } = AutoOffsetReset.Latest;

        /// <summary>
        /// Enable auto-commit.
        /// </summary>
        public bool EnableAutoCommit { get; set; } = false;

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

        /// <summary>
        /// Additional configuration.
        /// </summary>
        public IDictionary<string, string> AdditionalConfiguration { get; set; } = new Dictionary<string, string>();
    }
}