﻿namespace GiG.Core.Orleans.Streams.Kafka.Abstractions
{
    /// <summary>
    /// Kafka Settings for Orleans Streams.
    /// </summary>
    public class KafkaOptions
    {
        /// <summary>
        /// The configuration default section name.
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
        /// SSL Options for Kafka Provider.
        /// </summary>
        public KafkaSecurityOptions Security { get; set; } = new KafkaSecurityOptions();
    }
}