using GiG.Core.Data.KVStores.Abstractions;
using GiG.Core.Data.KVStores.Providers.FileProviders.Abstractions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace GiG.Core.Data.KVStores.Providers.FileProviders
{
    /// <inheritdoc />
    public class FileDataProvider<T> : IDataProvider<T>
    {
        private readonly ILogger<FileDataProvider<T>> _logger;
        private readonly IDataStore<T> _dataStore;
        private readonly IDataSerializer<T> _dataSerializer;
        private readonly IFileProvider _fileProvider;
        private readonly FileProviderOptions _fileOptions;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger">The <see cref="ILogger{FileDataProvider}"/> which will be used to log events by the provider.</param>
        /// <param name="dataStore">The <see cref="IDataStore{T}" /> which will be used to store the data retrieved by the provider.</param>
        /// <param name="dataSerializer">The <see cref="IDataProvider{T}"/> which will be used to deserialize data from file.</param>
        /// <param name="fileProvider">The <see cref="IFileProvider"/> which will be used to read data from file.</param>
        /// <param name="fileOptionsAccessor">The <see cref="IDataProviderOptions{T,TOptions}"/> which will be used to access options for the instance of the provider.</param>

        public FileDataProvider(ILogger<FileDataProvider<T>> logger,
            IDataStore<T> dataStore,
            IDataSerializer<T> dataSerializer,
            IFileProvider fileProvider, 
            IDataProviderOptions<T, FileProviderOptions> fileOptionsAccessor)
        {
            _logger = logger;
            _dataStore = dataStore;
            _dataSerializer = dataSerializer;
            _fileProvider = fileProvider;
            _fileOptions = fileOptionsAccessor.Value;
        }

        /// <inheritdoc/>
        public Task StartAsync()
        {
            _logger.LogDebug("Start Executed for {file}", _fileOptions.Path);

            var model = Load();

            _dataStore.Set(model);
            
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task StopAsync()
        {
            _logger.LogDebug("Stop Executed for {file}", _fileOptions.Path);

            return Task.CompletedTask;
        }

        private T Load()
        {
            var file = _fileProvider.GetFileInfo(_fileOptions.Path);
            if (file == null || !file.Exists)
                throw new InvalidOperationException($"File '{_fileOptions.Path}' does not exist");

            var model = default(T);
            
            using (var stream = file.CreateReadStream())
            {
                using (var reader = new StreamReader(stream))
                {
                    try
                    {
                        model = _dataSerializer.GetFromString(reader.ReadToEnd());
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, e.Message);
                    }
                }
            }

            return model;
        }
    }
}