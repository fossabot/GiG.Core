using GiG.Core.Data.KVStores.Abstractions;
using System.Collections.Concurrent;

namespace GiG.Core.Data.KVStores
{
    /// <inheritdoc />
    public class MemoryDataStore<T> : IDataStore<T>
    {
        private readonly ConcurrentDictionary<string, T> _data = new ConcurrentDictionary<string,T>();

        /// <inheritdoc />
        public T Get(params string[] keys)
        {
            var key = GetKey(keys);

            return _data.TryGetValue(key, out var value) ? value : default;
        }

        /// <inheritdoc />
        public void Set(T value, params string[] keys)
        {
            var key = GetKey(keys);
            
            _data.AddOrUpdate(key, value, (_, __) => value);
        }

        private static string GetKey(params string[] keys)
        {
            return string.Join(":", keys);
        }
    }
}