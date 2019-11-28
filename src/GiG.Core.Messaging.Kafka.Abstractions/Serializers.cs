using Confluent.Kafka;

namespace GiG.Core.Messaging.Kafka.Abstractions
{
    public class Serializers<TKey, TValue>
    {
        public ISerializer<TKey> KeySerializer { get; set; }
        
        public ISerializer<TValue> ValueSerializer { get; set; }

        public IAsyncSerializer<TValue> AsyncKeySerializer { get; set; }
        
        public IAsyncSerializer<TValue> AsyncValueSerializer { get; set; }

        public IDeserializer<TValue> KeyDeserializer { get; set; }
        
        public IDeserializer<TValue> ValueDeserializer { get; set; }
    }
}