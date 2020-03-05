using Bogus;
using GiG.Core.Data.KVStores.Abstractions;
using GiG.Core.Data.KVStores.Extensions;
using GiG.Core.Data.KVStores.Providers.FileProviders.Abstractions;
using GiG.Core.Data.KVStores.Providers.FileProviders.Extensions;
using GiG.Core.Data.KVStores.Providers.Tests.Integration.Mocks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Data.KVStores.Providers.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    public class FileProviderTests : IAsyncDisposable
    {
        private IHost _host;
        private IServiceProvider _serviceProvider;

        public FileProviderTests()
        {
            
        }

        private void InitForRead()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettingsFileRead.json");
                })
                .ConfigureServices((hostContext, services) =>
                {
                    var configuration = hostContext.Configuration;

                    services.AddKVStores<IEnumerable<MockLanguage>>()
                        .AddMemoryDataStore()
                        .FromFile(configuration, "Languages")
                        .WithJsonSerialization();
                })
                .Build();

            _serviceProvider = _host.Services;

            _host.Start();
        }

        private void InitForWrite()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettingsFileWrite.json");
                })
                .ConfigureServices((hostContext, services) =>
                {
                    var configuration = hostContext.Configuration;

                    services.AddKVStores<IEnumerable<MockLanguage>>()
                        .AddMemoryDataStore()
                        .FromFile(configuration, "Languages")
                        .WithJsonSerialization();
                })
                .Build();

            _serviceProvider = _host.Services;

            _host.Start();
        }

        [Fact]
        public void GetData_JsonFileProviderUsingRootKey_ReturnsMockLanguages()
        {
            // Arrange.
            InitForRead();
            var languages = ReadFromJson(_serviceProvider);
            
            var dataRetriever = _serviceProvider.GetRequiredService<IDataRetriever<IEnumerable<MockLanguage>>>();
            
            // Act.
            var data = dataRetriever.Get();
            
            // Assert.
            Assert.NotNull(data);
            Assert.Equal(languages.Select(l => l.Alpha2Code), data.Select(l => l.Alpha2Code));
        }

        [Fact]
        public async Task GetData_JsonFileProviderUsingValidKey_ReturnsMockLanguages()
        {
            // Arrange.
            InitForRead();
            var languages = ReadFromJson(_serviceProvider);

            var dataRetriever = _serviceProvider.GetRequiredService<IDataRetriever<IEnumerable<MockLanguage>>>();

            // Act.
            var data = await dataRetriever.GetAsync("temp");

            // Assert.
            Assert.NotNull(data);
            Assert.Equal(languages.Select(l => l.Alpha2Code), data.Select(l => l.Alpha2Code));
        }

        [Fact] 
        public async Task GetData_JsonFileProviderUsingInvalidKey_ReturnsMockLanguages()
        {
            // Arrange.
            InitForRead();
            var languages = ReadFromJson(_serviceProvider);

            var dataRetriever = _serviceProvider.GetRequiredService<IDataRetriever<IEnumerable<MockLanguage>>>();

            // Act.
            var data = await dataRetriever.GetAsync("temp2");

            // Assert.
            Assert.NotNull(data);
            Assert.Equal(languages.Select(l => l.Alpha2Code), data.Select(l => l.Alpha2Code));
        }

        [Fact]
        public async Task WriteAsync_JsonFileProviderUsingRootKey_Success()
        {
            // Arrange.
            InitForWrite();
            var dataProvider = _serviceProvider.GetRequiredService<IDataProvider<IEnumerable<MockLanguage>>>();

            var languages = new Faker<MockLanguage>()
                .RuleFor(x => x.Alpha2Code, new Randomizer().String(2, 'a', 'z'))
                .RuleFor(x => x.Name, new Randomizer().String(100, 'a', 'z'))
                .Generate(5)
                .AsEnumerable();

            await dataProvider.WriteAsync(languages);

            // Act.
            IEnumerable<MockLanguage> actualData = await dataProvider.GetAsync();

            // Assert.
            Assert.NotNull(actualData);
            Assert.Equal(languages.Select(l => l.Alpha2Code), actualData.Select(l => l.Alpha2Code));
        }

        [Fact]
        public async Task WriteAsync_JsonFileProviderUsingValidKey_Success()
        {
            // Arrange.
            InitForWrite();
            var dataProvider = _serviceProvider.GetRequiredService<IDataProvider<IEnumerable<MockLanguage>>>();

            var languages = new Faker<MockLanguage>()
                .RuleFor(x => x.Alpha2Code, new Randomizer().String(2, 'a', 'z'))
                .RuleFor(x => x.Name, new Randomizer().String(100, 'a', 'z'))
                .Generate(5)
                .AsEnumerable();

            await dataProvider.WriteAsync(languages, "temp");

            // Act.
            IEnumerable<MockLanguage> actualData = await dataProvider.GetAsync("temp");

            // Assert.
            Assert.NotNull(actualData);
            Assert.Equal(languages.Select(l => l.Alpha2Code), actualData.Select(l => l.Alpha2Code));
        }

        [Fact]
        public async Task WriteAsync_JsonFileProviderUsingInvalidKey_Success()
        {
            // Arrange.
            InitForWrite();
            var dataProvider = _serviceProvider.GetRequiredService<IDataProvider<IEnumerable<MockLanguage>>>();

            var languages = new Faker<MockLanguage>()
                .RuleFor(x => x.Alpha2Code, new Randomizer().String(2, 'a', 'z'))
                .RuleFor(x => x.Name, new Randomizer().String(100, 'a', 'z'))
                .Generate(5)
                .AsEnumerable();

            await dataProvider.WriteAsync(languages, "temp2");

            // Act.
            IEnumerable<MockLanguage> actualData = await dataProvider.GetAsync("temp2");

            // Assert.
            Assert.NotNull(actualData);
            Assert.Equal(languages.Select(l => l.Alpha2Code), actualData.Select(l => l.Alpha2Code));
        }

        private IEnumerable<MockLanguage> ReadFromJson(IServiceProvider serviceProvider)
        {
            var optionsAccessor = serviceProvider.GetRequiredService<IDataProviderOptions<IEnumerable<MockLanguage>,FileProviderOptions>>();
            var fileProvider = serviceProvider.GetRequiredService<IFileProvider>();
            
            var file = fileProvider.GetFileInfo(optionsAccessor.Value.Path);
            if (file == null || !file.Exists)
                throw new InvalidOperationException($"File '{optionsAccessor.Value.Path}' does not exist");

            return JsonSerializer.DeserializeAsync<IEnumerable<MockLanguage>>(file.CreateReadStream()).Result;
        }

        public async ValueTask DisposeAsync()
        {
            await _host.StopAsync();
        }
    }
}