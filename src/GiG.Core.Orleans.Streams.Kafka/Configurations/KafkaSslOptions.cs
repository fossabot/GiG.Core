﻿using Orleans.Streams.Kafka.Config;

namespace GiG.Core.Orleans.Streams.Kafka.Configurations
{
    /// <summary>
    /// Kafka Stream Provider Ssl Options
    /// </summary>
    public class KafkaSslOptions
    {
        /// <summary>
        /// Enable or Disable SSL Options.
        /// </summary>
        public bool IsEnabled { get; set; }
        
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