using GiG.Core.Messaging.Kafka.Abstractions.Interfaces;
using Microsoft.Extensions.Options;

namespace GiG.Core.Messaging.Kafka.Abstractions
{
    /// <inheritdoc cref="IKafkaBuilderOptions{TKey,TValue}" />
    public class KafkaBuilderOptions<TKey, TValue> : IOptions<KafkaBuilderOptions<TKey, TValue>>, IKafkaBuilderOptions<TKey, TValue>
    {
        /// <summary>
        /// Initializes a new instance of the KafkaBuilderOptions class.
        /// </summary>
        public KafkaBuilderOptions() { }

        /// <summary>
        /// Initializes a new instance of the KafkaBuilderOptions class.
        /// </summary>
        /// <param name="messageFactory"></param>
        public KafkaBuilderOptions(IMessageFactory messageFactory) => MessageFactory = messageFactory;
        
        /// <inheritdoc />
        public KafkaProviderOptions KafkaProviderOptions { get; set; } = new KafkaProviderOptions();
        
        /// <inheritdoc />
        public Serializers<TKey, TValue> Serializers { get; set; }
        
        /// <inheritdoc />
        public IMessageFactory MessageFactory { get; set; }

        /// <inheritdoc />
        public KafkaBuilderOptions<TKey, TValue> Value => this;
    }
}