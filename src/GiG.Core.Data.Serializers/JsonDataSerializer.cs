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
        public T Deserialize(string value)
        {
            return string.IsNullOrEmpty(value) ? default : JsonSerializer.Deserialize<T>(value);
        }

        /// <inheritdoc />
        public string Serialize(T model)
        {
            return JsonSerializer.Serialize(model);
        }
    }
}