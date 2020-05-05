using Confluent.Kafka;
using System;
using System.Text;
using System.Text.Json;

namespace GiG.Core.Messaging.Kafka.Serializers
{
    /// <summary>
    /// JSON deserializer for use with <see cref="T:Confluent.Kafka.Consumer`2" />.
    /// </summary>
    public class JsonDeserializer<T> : IDeserializer<T> 
    {
        /// <inheritdoc />
        public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            if (isNull)
            {
                return default;
            }

            var deserializedDataAsJson = Encoding.UTF8.GetString(data.ToArray());

            return JsonSerializer.Deserialize<T>(deserializedDataAsJson);
        }
    }
}