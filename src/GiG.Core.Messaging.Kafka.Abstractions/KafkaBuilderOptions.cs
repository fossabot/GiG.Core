using GiG.Core.Messaging.Kafka.Abstractions.Interfaces;
using Microsoft.Extensions.Options;

namespace GiG.Core.Messaging.Kafka.Abstractions
{
    public class KafkaBuilderOptions<TKey, TValue> : IOptions<KafkaBuilderOptions<TKey, TValue>>, IKafkaBuilderOptions<TKey, TValue>
    {
        public KafkaProviderOptions KafkaProviderOptions { get; set; } = new KafkaProviderOptions();
        
        public Serializers<TKey, TValue> Serializers { get; set; }
        
        public IMessageFactory MessageFactory { get; set; }

        public KafkaBuilderOptions<TKey, TValue> Value => this;

        public KafkaBuilderOptions() { }

        public KafkaBuilderOptions(IMessageFactory messageFactory) => MessageFactory = messageFactory;
    }
}