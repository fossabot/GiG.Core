// ReSharper disable UnusedTypeParameter

namespace GiG.Core.Data.KVStores.Abstractions
{
    /// <summary>
    /// DataProviderOptions.
    /// </summary>
    /// <typeparam name="T">Generic to define type of data provider. </typeparam>
    /// <typeparam name="TOptions">Generic to define type of data provider options. </typeparam>
    public interface IDataProviderOptions<T, out TOptions>
    {
        /// <summary>
        /// Value of options.
        /// </summary>
        TOptions Value { get; }
    }
}