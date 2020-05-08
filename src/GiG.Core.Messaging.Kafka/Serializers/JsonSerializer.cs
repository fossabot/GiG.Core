using Confluent.Kafka;
using System.Text;
using System.Text.Json;

namespace GiG.Core.Messaging.Kafka.Serializers
{
    /// <summary>
    /// JSON serializer for use with <see cref="T:Confluent.Kafka.Producer`2" />.
    /// </summary>
    public class JsonSerializer<T> : ISerializer<T>
    {
        /// <inheritdoc />
        public byte[] Serialize(T data, SerializationContext context)
        {
            if (data == null)
            {
                return null;
            }

            var serializedDataAsJson = JsonSerializer.Serialize(data);
            
            return Encoding.UTF8.GetBytes(serializedDataAsJson);
        }
    }
}