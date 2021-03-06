﻿namespace GiG.Core.Data.KVStores.Abstractions
{
    /// <inheritdoc />
    public class DataProviderOptions<T, TOptions> : IDataProviderOptions<T, TOptions>
    {
        /// <summary>
        /// Initializes a new instance of the DataProviderOptions class.
        /// </summary>
        /// <param name="options">The provider options.</param>
        public DataProviderOptions(TOptions options)
        {
            Value = options;
        }

        /// <inheritdoc />
        public TOptions Value { get; }
    }
}