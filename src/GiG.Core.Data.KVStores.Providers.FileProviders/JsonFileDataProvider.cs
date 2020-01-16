using GiG.Core.Data.KVStores.Abstractions;
using GiG.Core.Data.KVStores.Providers.FileProviders.Abstractions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Text.Json;

namespace GiG.Core.Data.KVStores.Providers.FileProviders
{
    /// <inheritdoc />
    public class JsonFileDataProvider<T> : FileDataProvider<T>
    {
        /// <inheritdoc />
        public JsonFileDataProvider(
            ILogger<FileDataProvider<T>> logger, 
            IDataStore<T> dataStore, 
            IFileProvider fileProvider, 
            IDataProviderOptions<T, FileProviderOptions> fileOptionsAccessor) : 
            base(logger, dataStore, fileProvider, fileOptionsAccessor)
        {

        }

        /// <inheritdoc />
        protected override T GetFromStream(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                var json = reader.ReadToEnd();
                
                return JsonSerializer.Deserialize<T>(json);
            }
        }
    }
}