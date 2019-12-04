using Confluent.Kafka;
using GiG.Core.Messaging.Kafka.Abstractions.Extensions;
using GiG.Core.Messaging.Kafka.Abstractions.Interfaces;
using System.Collections.Generic;

namespace GiG.Core.Messaging.Kafka.Abstractions
{
    /// <inheritdoc />
    public class KafkaMessage<TKey, TValue> : IKafkaMessage<TKey, TValue>
    {
        /// <inheritdoc />
        public TKey Key { get; set; }

        /// <inheritdoc />
        public TValue Value { get; set; }

        /// <inheritdoc />
        public string MessageType { get; set; }

        /// <inheritdoc />
        public string MessageId { get; set; }

        internal TopicPartitionOffset Offset { get; set; }

        /// <inheritdoc />
        public IDictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Converts the specified <see cref="ConsumeResult{TKey, TValue}"/> to a <see cref="KafkaMessage{TKey, TValue}"/>.
        /// </summary>
        /// <param name="result">The kafka message, <see cref="KafkaMessage{TKey, TValue}"/>.</param>
        /// <returns></returns>
        public static implicit operator KafkaMessage<TKey, TValue>(ConsumeResult<TKey, TValue> result) =>
            new KafkaMessage<TKey, TValue>
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