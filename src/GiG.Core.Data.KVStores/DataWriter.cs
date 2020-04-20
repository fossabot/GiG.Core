using GiG.Core.Data.KVStores.Abstractions;
using GiG.Core.Data.KVStores.Abstractions.Exceptions;
using System;
using System.Threading.Tasks;

namespace GiG.Core.Data.KVStores
{
    /// <inheritdoc />
    public class DataWriter<T> : IDataWriter<T>
    {
        private readonly IDataProvider<T> _dataProvider;

        /// <summary>
        /// Initializes a new instance of the DataWriter class.
        /// </summary>
        /// <param name="dataProvider">The data provider used to write data to source.</param>
        public DataWriter(IDataProvider<T> dataProvider)
        {
            _dataProvider = dataProvider;
        }

        /// <inheritdoc />
        public async Task WriteAsync(T value, params string[] keys)
        {
            try
            {
                await _dataProvider.WriteAsync(value, keys);
            }
            catch (Exception ex)
            {
                throw new WriteException($"Cannot write value for {typeof(T).FullName} with keys {string.Join(", ", keys)}", ex);
            }
        }

        /// <inheritdoc />
        public async Task LockAsync(Func<Task> action, params string[] keys)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            var handle = await _dataProvider.LockAsync(keys);
            try
            {
                await action();
            }
            finally
            {
                await _dataProvider.UnlockAsync(handle);
            }
        }
    }
}