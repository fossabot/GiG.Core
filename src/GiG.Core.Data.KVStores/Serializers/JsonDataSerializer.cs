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
            return JsonSerializer.Deserialize<T>(value);
        }
    }
}