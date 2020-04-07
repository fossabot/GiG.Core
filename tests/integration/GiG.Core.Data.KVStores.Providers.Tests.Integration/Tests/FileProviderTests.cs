using Bogus;
using GiG.Core.Data.KVStores.Abstractions;
using GiG.Core.Data.KVStores.Extensions;
using GiG.Core.Data.KVStores.Providers.File.Abstractions;
using GiG.Core.Data.KVStores.Providers.File.Extensions;
using GiG.Core.Data.KVStores.Providers.Hosting;
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

        [Fact]
        public async Task GetData_JsonFileProviderUsingRootKey_ReturnsMockLanguages()
        {
            // Arrange.
            InitForRead();
            var languages = ReadFromJson(_serviceProvider);

            var dataRetriever = _serviceProvider.GetRequiredService<IDataRetriever<IEnumerable<MockLanguage>>>();

            // Act.
            var data = await dataRetriever.GetAsync();

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
            var dataRetriever = _serviceProvider.GetRequiredService<IDataRetriever<IEnumerable<MockLanguage>>>();
            var dataWriter = _serviceProvider.GetRequiredService<IDataWriter<IEnumerable<MockLanguage>>>();

            var languages = new Faker<MockLanguage>()
                .RuleFor(x => x.Alpha2Code, new Randomizer().String(2, 'a', 'z'))
                .RuleFor(x => x.Name, new Randomizer().String(100, 'a', 'z'))
                .Generate(5)
                .AsEnumerable();

            await dataWriter.WriteAsync(languages);

            // Act.
            var actualData = await dataRetriever.GetAsync();

            // Assert.
            Assert.NotNull(actualData);
            Assert.Equal(languages.Select(l => l.Alpha2Code), actualData.Select(l => l.Alpha2Code));
        }

        [Fact]
        public async Task WriteAsync_JsonFileProviderUsingValidKey_Success()
        {
            // Arrange.
            InitForWrite();
            var dataRetriever = _serviceProvider.GetRequiredService<IDataRetriever<IEnumerable<MockLanguage>>>();
            var dataWriter = _serviceProvider.GetRequiredService<IDataWriter<IEnumerable<MockLanguage>>>();

            var languages = new Faker<MockLanguage>()
                .RuleFor(x => x.Alpha2Code, new Randomizer().String(2, 'a', 'z'))
                .RuleFor(x => x.Name, new Randomizer().String(100, 'a', 'z'))
                .Generate(5)
                .AsEnumerable();

            await dataWriter.WriteAsync(languages, "temp");

            // Act.
            var actualData = await dataRetriever.GetAsync("temp");

            // Assert.
            Assert.NotNull(actualData);
            Assert.Equal(languages.Select(l => l.Alpha2Code), actualData.Select(l => l.Alpha2Code));
        }

        [Fact]
        public async Task WriteAsync_JsonFileProviderUsingInvalidKey_Success()
        {
            // Arrange.
            InitForWrite();
            var dataRetriever = _serviceProvider.GetRequiredService<IDataRetriever<IEnumerable<MockLanguage>>>();
            var dataWriter = _serviceProvider.GetRequiredService<IDataWriter<IEnumerable<MockLanguage>>>();

            var languages = new Faker<MockLanguage>()
                .RuleFor(x => x.Alpha2Code, new Randomizer().String(2, 'a', 'z'))
                .RuleFor(x => x.Name, new Randomizer().String(100, 'a', 'z'))
                .Generate(5)
                .AsEnumerable();

            await dataWriter.WriteAsync(languages, "temp2");

            // Act.
            IEnumerable<MockLanguage> actualData = await dataRetriever.GetAsync("temp2");

            // Assert.
            Assert.NotNull(actualData);
            Assert.Equal(languages.Select(l => l.Alpha2Code), actualData.Select(l => l.Alpha2Code));
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

                    services.AddKVStores<IEnumerable<MockLanguage>>(x =>
                        x.FromFile(configuration, "Languages")
                            .WithEagerLoading());
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

                    services.AddKVStores<IEnumerable<MockLanguage>>(x =>
                        x.FromFile(configuration, "Languages"));
                })
                .Build();

            _serviceProvider = _host.Services;

            _host.Start();
        }

        private static IEnumerable<MockLanguage> ReadFromJson(IServiceProvider serviceProvider)
        {
            var optionsAccessor = serviceProvider
                .GetRequiredService<IDataProviderOptions<IEnumerable<MockLanguage>, FileProviderOptions>>();

            var file = new PhysicalFileProvider(AppContext.BaseDirectory).GetFileInfo(optionsAccessor.Value.Path);

            return JsonSerializer.DeserializeAsync<IEnumerable<MockLanguage>>(file.CreateReadStream()).Result;
        }

        public async ValueTask DisposeAsync()
        {
            await _host.StopAsync();
        }
    }
}