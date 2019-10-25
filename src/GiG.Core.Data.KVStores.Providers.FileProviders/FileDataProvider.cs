using GiG.Core.Data.KVStores.Abstractions;
using GiG.Core.Data.KVStores.Providers.FileProviders.Abstractions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;

namespace GiG.Core.Data.KVStores.Providers.FileProviders
{
    public abstract class FileDataProvider<T> : IDataProvider<T>
    {
        private readonly ILogger<FileDataProvider<T>> _logger;
        private readonly IDataStore<T> _dataStore;
        private readonly IFileProvider _fileProvider;
        private readonly IOptions<FileProviderOptions> _fileOptionsAccessor;

        protected FileDataProvider(ILogger<FileDataProvider<T>> logger,
            IDataStore<T> dataStore, 
            IFileProvider fileProvider, IOptions<FileProviderOptions> fileOptionsAccessor)
        {
            _logger = logger;
            _dataStore = dataStore;
            _fileProvider = fileProvider;
            _fileOptionsAccessor = fileOptionsAccessor;
        }

        public Task StartAsync()
        {
            var model = Load();

            _dataStore.Set(model);
            
            return Task.CompletedTask;
        }

        public Task StopAsync()
        {
            return Task.CompletedTask;
        }

        private T Load()
        {
            var file = _fileProvider.GetFileInfo(_fileOptionsAccessor.Value.Path);
            if (file == null || !file.Exists)
                throw new InvalidOperationException($"File '{_fileOptionsAccessor.Value.Path}' does not exist");

            var model = default(T);
            
            using (var stream = file.CreateReadStream())
            {
                try
                {
                    model = GetFromStream(stream);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                }
            }

            return model;
        }

        protected abstract T GetFromStream(Stream stream);
    }
}