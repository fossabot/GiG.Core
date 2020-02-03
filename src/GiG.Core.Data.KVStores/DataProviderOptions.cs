using GiG.Core.Data.KVStores.Abstractions;

namespace GiG.Core.Data.KVStores
{
    /// <inheritdoc />
    public class DataProviderOptions<T, TOptions> : IDataProviderOptions<T, TOptions>
    {
        /// <summary>
        /// Initializes a new instance of the DataProviderOptions class.
        /// </summary>
        /// <param name="options"></param>
        public DataProviderOptions(TOptions options)
        {
            Value = options;
        }

        /// <inheritdoc />
        public TOptions Value { get; }
    }
}