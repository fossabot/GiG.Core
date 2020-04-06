using GiG.Core.Data.KVStores.Abstractions;
using System.Text.Json;

namespace GiG.Core.Data.KVStores.Serializers
{
    /// <inheritdoc />
    public class JsonDataSerializer<T> : IDataSerializer<T>
    {
        /// <inheritdoc />
        public T GetFromString(string value)
        {
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }

        /// <inheritdoc />
        public string ConvertToString(T model)
        {
            return JsonSerializer.Serialize(model);
        }
    }
}