using GiG.Core.Data.KVStores.Abstractions;
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
            var handle = await _dataProvider.LockAsync(keys);

            await _dataProvider.WriteAsync(value, keys);

            await _dataProvider.UnlockAsync(handle);
        }
    }
}