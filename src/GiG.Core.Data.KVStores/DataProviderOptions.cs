using GiG.Core.Data.KVStores.Abstractions;

namespace GiG.Core.Data.KVStores
{
    /// <inheritdoc />
    public class DataProviderOptions<T, TOptions> : IDataProviderOptions<T, TOptions>
    {
        /// <inheritdoc />
        public DataProviderOptions(TOptions options)
        {
            Value = options;
        }

        /// <inheritdoc />
        public TOptions Value { get; }
    }
}