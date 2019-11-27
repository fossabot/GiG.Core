using Confluent.Kafka;
using GiG.Core.Messaging.Kafka.Abstractions.Extensions;
using GiG.Core.Messaging.Kafka.Abstractions.Interfaces;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("GiG.Core.Messaging.Kafka")]
namespace GiG.Core.Messaging.Kafka.Abstractions
{
    public class KafkaMessage<TKey, TValue> : IKafkaMessage<TKey, TValue>
    {
        public TKey Key { get; set; }

        public TValue Value { get; set; }

        public string MessageType { get; set; }

        public string MessageId { get; set; }

        internal TopicPartitionOffset Offset { get; set; }

        public IDictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();

        public static implicit operator KafkaMessage<TKey, TValue>(ConsumeResult<TKey, TValue> result)
        {
            return new KafkaMessage<TKey, TValue>
            {
                Key = result.Message.Key,
                Value = result.Message.Value,
                MessageType = result.Message.Headers.GetHeaderValue(KafkaConstants.MessageTypeHeaderName),
                MessageId = result.Message.Headers.GetHeaderValue(KafkaConstants.MessageIdHeaderName),
                Headers = result.Message.Headers.AsDictionary(),
                Offset = result.TopicPartitionOffset
            };
        }
    }
}