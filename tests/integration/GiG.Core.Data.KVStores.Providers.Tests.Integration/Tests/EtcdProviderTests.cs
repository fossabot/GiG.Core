using dotnet_etcd;
using GiG.Core.Data.KVStores.Abstractions;
using GiG.Core.Data.KVStores.Extensions;
using GiG.Core.Data.KVStores.Providers.Etcd.Abstractions;
using GiG.Core.Data.KVStores.Providers.Etcd.Extensions;
using GiG.Core.Data.KVStores.Providers.Tests.Integration.Mocks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Data.KVStores.Providers.Tests.Integration.Tests
{
    [Trait("Category", "IntegrationWithDependency")]
    public class EtcdProviderTests : IAsyncDisposable
    {
        private readonly IHost _host;
        private readonly IServiceProvider _serviceProvider;
        private readonly EtcdClient _etcdClient;
        private readonly EtcdProviderOptions _etcdProviderOptions;

        public EtcdProviderTests()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettingsEtcd.json");
                })
                .ConfigureServices((hostContext, services) =>
                {
                    var hostContextConfiguration = hostContext.Configuration;

                    services.AddKVStores<IEnumerable<MockLanguage>>()
                        .FromEtcd(hostContextConfiguration, "Languages")
                        .WithJsonSerialization();
                })
                .Build();

            _serviceProvider = _host.Services;

            var configuration = _serviceProvider.GetRequiredService<IConfiguration>();

            var configurationSection = configuration.GetSection("Languages");

            _etcdProviderOptions = configurationSection.Get<EtcdProviderOptions>();

            _etcdClient = new EtcdClient(_etcdProviderOptions.ConnectionString);

            _host.Start();
        }

        [Fact]
        public async Task GetData_EtcdProviderUsingRootKey_ReturnsMockLanguages()
        {
            // Arrange.
            await ClearEtcdKey(_etcdProviderOptions.Key);
            await WriteToEtcd("languages.json", _etcdProviderOptions.Key);

            var languages = await ReadFromEtcd(_etcdProviderOptions.Key);

            var dataRetriever = _serviceProvider.GetRequiredService<IDataRetriever<IEnumerable<MockLanguage>>>();

            // Act.
            IEnumerable<MockLanguage> data = dataRetriever.Get();

            // Assert.
            Assert.NotNull(data);
            Assert.Equal(languages.Select(l => l.Alpha2Code), data.Select(l => l.Alpha2Code));
        }

        [Fact]
        public async Task GetData_EtcdProviderUsingValidKey_ReturnsMockLanguages()
        {
            // Arrange.
            await ClearEtcdKey(_etcdProviderOptions.Key);
            await WriteToEtcd("languages.json", string.Concat(_etcdProviderOptions.Key, "/temp"));

            var languages = await ReadFromEtcd(_etcdProviderOptions.Key);

            var dataRetriever = _serviceProvider.GetRequiredService<IDataRetriever<IEnumerable<MockLanguage>>>();

            // Act.
            IEnumerable<MockLanguage> data = await dataRetriever.GetAsync("temp");

            // Assert.
            Assert.NotNull(data);
            Assert.Equal(languages.Select(l => l.Alpha2Code), data.Select(l => l.Alpha2Code));
        }

        [Fact]
        public async Task GetData_EtcdProviderUsingInvalidKey_ReturnsMockLanguages()
        {
            // Arrange.
            await ClearEtcdKey(_etcdProviderOptions.Key);
            await WriteToEtcd("languages.json", string.Concat(_etcdProviderOptions.Key, "/temp"));

            var languages = await ReadFromEtcd(_etcdProviderOptions.Key);

            var dataRetriever = _serviceProvider.GetRequiredService<IDataRetriever<IEnumerable<MockLanguage>>>();

            // Act.
            IEnumerable<MockLanguage> data = await dataRetriever.GetAsync("temp2");

            // Assert.
            Assert.NotNull(data);
            Assert.Equal(languages.Select(l => l.Alpha2Code), data.Select(l => l.Alpha2Code));
        }

        public async ValueTask DisposeAsync()
        {
            await _host.StopAsync();
        }

        private async Task ClearEtcdKey(string key)
        {
            await _etcdClient.DeleteAsync(key);
        }

        private async Task WriteToEtcd(string filePath, string key)
        {
            var fileInfo = new FileInfo(filePath);

            if (!fileInfo.Exists)
                throw new InvalidOperationException($"File '{fileInfo.FullName}' does not exist");

            await _etcdClient.PutAsync(key, fileInfo.OpenText().ReadToEnd());
        }

        private async Task<IEnumerable<MockLanguage>> ReadFromEtcd(string key)
        {
            var value = await _etcdClient.GetValAsync(key);

            return JsonSerializer.Deserialize<IEnumerable<MockLanguage>>(value);
        }
    }
}