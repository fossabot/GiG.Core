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

        /// <inheritdoc />
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
            _logger.LogInformation("Start Executed for {file}", _fileOptions.Path);

            var model = Load();

            _dataStore.Set(model);
            
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task StopAsync()
        {
            _logger.LogInformation("Stop Executed for {file}", _fileOptions.Path);

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