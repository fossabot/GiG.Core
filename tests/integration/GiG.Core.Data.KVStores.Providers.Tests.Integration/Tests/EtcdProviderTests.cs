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
                .ConfigureAppConfiguration((hostingContext, config) => { config.AddJsonFile("appsettingsEtcd.json"); })
                .ConfigureServices((hostContext, services) =>
                {
                    var configuration = hostContext.Configuration;

                    services.AddKVStores<IEnumerable<MockLanguage>>()
                        .FromEtcd(configuration, "Languages")
                        .WithJsonSerialization();
                })
                .Build();

            _serviceProvider = _host.Services;

            var configuration = _serviceProvider.GetRequiredService<IConfiguration>();

            var configurationSection = configuration.GetSection("Languages");

            _etcdProviderOptions = configurationSection.Get<EtcdProviderOptions>();

            _etcdClient = new EtcdClient(_etcdProviderOptions.ConnectionString, _etcdProviderOptions.Port,
                _etcdProviderOptions.Username, _etcdProviderOptions.Password, _etcdProviderOptions.CaCertificate,
                _etcdProviderOptions.ClientCertificate, _etcdProviderOptions.ClientKey,
                _etcdProviderOptions.IsPublicRootCa);
            
            _host.Start();
        }

        [Fact]
        public async Task GetData_EtcdProviderUsingRootKey_ReturnsMockLanguages()
        {
            // Arrange.
            var key = _etcdProviderOptions.Key;
      
            await WriteToEtcd("languages.json", key);

            var languages = await ReadFromEtcd(key);

            var dataRetriever = _serviceProvider.GetRequiredService<IDataRetriever<IEnumerable<MockLanguage>>>();

            // Act.
            IEnumerable<MockLanguage> data = await dataRetriever.GetAsync();

            // Assert.
            Assert.NotNull(data);
            Assert.Equal(languages.Select(l => l.Alpha2Code), data.Select(l => l.Alpha2Code));
        }

        [Fact]
        public async Task GetData_EtcdProviderUsingValidKey_ReturnsMockLanguages()
        {
            // Arrange.
            var tenantId = Guid.NewGuid().ToString();
            
            var key = string.Concat(_etcdProviderOptions.Key, "/", tenantId);
   
            await WriteToEtcd("languages.json", key);

            var languages = await ReadFromEtcd(key);

            var dataRetriever = _serviceProvider.GetRequiredService<IDataRetriever<IEnumerable<MockLanguage>>>();

            // Act.
            IEnumerable<MockLanguage> data = await dataRetriever.GetAsync(tenantId);

            // Assert.
            Assert.NotNull(data);
            Assert.Equal(languages.Select(l => l.Alpha2Code), data.Select(l => l.Alpha2Code));
        }

        [Fact]
        public async Task GetData_EtcdProviderUsingInvalidKey_ReturnsMockLanguages()
        {
            // Arrange.
            var key = _etcdProviderOptions.Key;
        
            await WriteToEtcd("languages.json", key);

            var languages = await ReadFromEtcd(key);

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

            return string.IsNullOrEmpty(value) ? new List<MockLanguage>() : JsonSerializer.Deserialize<IEnumerable<MockLanguage>>(value);
        }
    }
}