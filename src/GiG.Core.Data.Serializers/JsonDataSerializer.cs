using GiG.Core.Data.Serializers.Abstractions;
using System.Text.Json;

namespace GiG.Core.Data.Serializers
{
    /// <summary>
    /// Json Data Serializer.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class JsonDataSerializer<T> : IDataSerializer<T>
    {
        /// <inheritdoc />
        public T Deserialize(string data)
        {
            return string.IsNullOrEmpty(data) ? default : JsonSerializer.Deserialize<T>(data);
        }

        /// <inheritdoc />
        public string Serialize(T data)
        {
            return JsonSerializer.Serialize(data);
        }
    }
}