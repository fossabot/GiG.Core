using Confluent.Kafka;
using Newtonsoft.Json;
using System.Text;

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

            var serializedDataAsJson = JsonConvert.SerializeObject(data);
            
            return Encoding.UTF8.GetBytes(serializedDataAsJson);
        }
    }
}