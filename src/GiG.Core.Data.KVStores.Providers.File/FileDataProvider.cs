using GiG.Core.Data.KVStores.Abstractions;
using GiG.Core.Data.KVStores.Providers.File.Abstractions;
using GiG.Core.Data.Serializers.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GiG.Core.Data.KVStores.Providers.File
{
    /// <inheritdoc />
    public class FileDataProvider<T> : IDataProvider<T>
    {
        private readonly IDataSerializer<T> _dataSerializer;
        private readonly IList<FileSystemWatcher> _watchers = new List<FileSystemWatcher>();
        private readonly IList<FileSystemEventHandler> _handlers =new List<FileSystemEventHandler>();

        private readonly string _fileName;
        private readonly string _fileExtension;
        private readonly FileInfo _fileInfo;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="dataSerializer">The <see cref="IDataProvider{T}"/> which will be used to serialize data from file.</param>
        /// <param name="fileOptionsAccessor">The <see cref="IDataProviderOptions{T,TOptions}"/> which will be used to access options for the instance of the provider.</param>

        public FileDataProvider(
            IDataSerializer<T> dataSerializer,
            IDataProviderOptions<T, FileProviderOptions> fileOptionsAccessor)
        {
            _dataSerializer = dataSerializer;

            var fileOptions = fileOptionsAccessor.Value;
            _fileInfo = new FileInfo(fileOptions.Path);
            if (!_fileInfo.Exists)
            {
                throw new InvalidOperationException($"File '{_fileInfo.FullName}' does not exist");
            }

            _fileName = fileOptions.Path;
            _fileExtension = _fileInfo.Extension;
        }

        /// <inheritdoc />
        public Task WatchAsync(Action<T> callback, params string[] keys)
        {
            var fileSystemWatcher = new FileSystemWatcher()
            {
                Path = _fileInfo.Directory?.FullName,
                NotifyFilter = NotifyFilters.LastWrite,
                Filter = _fileInfo.Name,
                EnableRaisingEvents = true
            };            
            
            void Callback(object sender, FileSystemEventArgs args)
            {
                var file = System.IO.File.ReadAllText(args.FullPath);
                callback(_dataSerializer.Deserialize(file));
            }

            fileSystemWatcher.Created += Callback;
            fileSystemWatcher.Changed += Callback;
            
            _handlers.Add(Callback);
            _watchers.Add(fileSystemWatcher);
            
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public async Task<T> GetAsync(params string[] keys)
        {
            var filePath = GetPath(keys);

            if (!System.IO.File.Exists(filePath))
            {
                return default;
            }
            
            var file = await System.IO.File.ReadAllTextAsync(filePath);
            
            return _dataSerializer.Deserialize(file);
        }

        /// <inheritdoc/>
        public Task WriteAsync(T model, params string[] keys)
        {
            var fileName = GetPath(keys);

            System.IO.File.WriteAllTextAsync(fileName, _dataSerializer.Serialize(model));

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task<object> LockAsync(params string[] keys)
        {
            return Task.FromResult<object>(null);
        }

        /// <inheritdoc />
        public Task UnlockAsync(object handle)
        {
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public void Dispose()
        {                
            foreach (var watcher in _watchers)
            {
                foreach (var handler in _handlers)
                {
                    watcher.Created -= handler;
                    watcher.Changed -= handler;
                }

                watcher.Dispose();
            }
        }

        private string GetPath(params string[] keys)
        {
            return keys.Any()
                ? string.Concat(_fileName.Replace(_fileExtension, string.Empty), ".", string.Join(".", keys), _fileExtension)
                : _fileName;
        }
    }
}