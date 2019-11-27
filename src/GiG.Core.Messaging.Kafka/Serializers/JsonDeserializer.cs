using Confluent.Kafka;
using Newtonsoft.Json;
using System;
using System.Text;

namespace GiG.Core.Messaging.Kafka.Serializers
{
    public class JsonDeserializer<T> : IDeserializer<T> 
    {
        public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            if (isNull)
            {
                return default;
            }

            var deserializedDataAsJson = Encoding.UTF8.GetString(data.ToArray());

            return JsonConvert.DeserializeObject<T>(deserializedDataAsJson);
        }
    }
}