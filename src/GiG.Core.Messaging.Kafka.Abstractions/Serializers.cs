using Confluent.Kafka;

namespace GiG.Core.Messaging.Kafka.Abstractions
{
    /// <summary>
    /// Kay and Value Serializers.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class Serializers<TKey, TValue>
    {
        /// <summary>
        /// Synchronous Key Serializer.
        /// </summary>
        public ISerializer<TKey> KeySerializer { get; set; }
        
        /// <summary>
        /// Synchronous Value Serializer.
        /// </summary>
        public ISerializer<TValue> ValueSerializer { get; set; }

        /// <summary>
        /// Asynchronous Key Serializer.
        /// </summary>
        public IAsyncSerializer<TKey> AsyncKeySerializer { get; set; }
        
        /// <summary>
        /// Asynchronous Value Serializer.
        /// </summary>
        public IAsyncSerializer<TValue> AsyncValueSerializer { get; set; }

        /// <summary>
        /// Synchronous Key Deserializer.
        /// </summary>
        public IDeserializer<TKey> KeyDeserializer { get; set; }
        
        /// <summary>
        /// Synchronous Value Deserializer.
        /// </summary>
        public IDeserializer<TValue> ValueDeserializer { get; set; }
        
        /// <summary>
        /// Asynchronous Key Deserializer.
        /// </summary>
        public IAsyncDeserializer<TKey> AsyncKeyDeserializer { get; set; }
        
        /// <summary>
        /// Asynchronous Value Deserializer.
        /// </summary>
        public IAsyncDeserializer<TValue> AsyncValueDeserializer { get; set; }
    }
}