﻿using Confluent.Kafka;
using System.Collections.Generic;

namespace GiG.Core.Messaging.Kafka.Abstractions.Extensions
{
    public class KafkaProviderOptions
    {
        public const string DefaultSectionName = "EventProvider";

        public string SchemaRegistry { get; set; } = "localhost:8081";

        public string BootstrapServers { get; set; } = "localhost:9092";

        public string GroupId { get; set; } = "default-group";

        public string Topic { get; set; } = "default-topic";

        public int MessageTimeoutMs { get; set; } = 25000;

        public AutoOffsetReset AutoOffsetReset { get; set; } = AutoOffsetReset.Latest;

        public bool EnableAutoCommit { get; set; } = false;

        public string SaslUsername { get; set; }

        public string SaslPassword { get; set; }

        public SecurityProtocol SecurityProtocol { get; set; } = SecurityProtocol.Plaintext;

        public SaslMechanism SaslMechanism { get; set; } = SaslMechanism.Plain;

        public IDictionary<string, string> AdditionalConfiguration { get; set; } = new Dictionary<string, string>();
    }
}