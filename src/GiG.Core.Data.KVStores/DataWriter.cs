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
        public Task WriteAsync(T value, params string[] keys)
        {
            return _dataProvider.WriteAsync(value, keys);
        }
    }
}