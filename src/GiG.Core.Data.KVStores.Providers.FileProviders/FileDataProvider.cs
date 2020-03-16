﻿using GiG.Core.Data.KVStores.Abstractions;
using GiG.Core.Data.KVStores.Providers.FileProviders.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GiG.Core.Data.KVStores.Providers.FileProviders
{
    /// <inheritdoc />
    public class FileDataProvider<T> : IDataProvider<T>
    {
        private readonly ILogger<FileDataProvider<T>> _logger;
        private readonly IDataStore<T> _dataStore;
        private readonly IDataSerializer<T> _dataSerializer;
        private readonly FileProviderOptions _fileOptions;

        private string _fileName;
        private string _fileExtension;

        private readonly object _fileLock = new object();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger">The <see cref="ILogger{FileDataProvider}"/> which will be used to log events by the provider.</param>
        /// <param name="dataStore">The <see cref="IDataStore{T}" /> which will be used to store the data retrieved by the provider.</param>
        /// <param name="dataSerializer">The <see cref="IDataProvider{T}"/> which will be used to deserialize data from file.</param>
        /// <param name="fileOptionsAccessor">The <see cref="IDataProviderOptions{T,TOptions}"/> which will be used to access options for the instance of the provider.</param>

        public FileDataProvider(ILogger<FileDataProvider<T>> logger,
            IDataStore<T> dataStore,
            IDataSerializer<T> dataSerializer,
            IDataProviderOptions<T, FileProviderOptions> fileOptionsAccessor)
        {
            _logger = logger;
            _dataStore = dataStore;
            _dataSerializer = dataSerializer;
            _fileOptions = fileOptionsAccessor.Value;
        }

        /// <inheritdoc/>
        public async Task StartAsync()
        {
            _fileName = _fileOptions.Path;

            var fileInfo = new FileInfo(_fileName);

            if (!fileInfo.Exists)
                throw new InvalidOperationException($"File '{fileInfo.FullName}' does not exist");

            _fileExtension = fileInfo.Extension;

            _logger.LogDebug("Start Executed for {file}", _fileOptions.Path);

            var model = await GetAsync();

            _dataStore.Set(model);
        }

        /// <summary>
        /// Retrieves a model from storage using list of keys. Each key is delimited by a "." and used to retrieve a subsection of the store.
        /// </summary>
        /// <param name="keys">The list of keys.</param>
        /// <returns></returns>
        public async Task<T> GetAsync(params string[] keys)
        {       
            var fileName = GetFileName(keys);

            var model = default(T);

            var fileInfo = new FileInfo(fileName);

            if (fileInfo.Exists)
            {
                using (var reader = fileInfo.OpenText())
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
            
            return await Task.FromResult(model);
        }

        /// <inheritdoc/>
        public Task WriteAsync(T model, params string[] keys)
        {
            var fileName = GetFileName(keys);

            lock (_fileLock)
            {
                File.WriteAllTextAsync(fileName, _dataSerializer.ConvertToString(model));
            }

            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task StopAsync()
        {
            _logger.LogDebug("Stop Executed for {file}", _fileOptions.Path);

            return Task.CompletedTask;
        }

        private string GetFileName(params string[] keys)
        {
            return keys.Any()
                ? string.Concat(_fileName.Replace(_fileExtension, string.Empty), ".", string.Join(".", keys), _fileExtension)
                : _fileName;
        }
    }
}