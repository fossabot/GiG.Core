using dotnet_etcd;
using GiG.Core.Data.KVStores.Abstractions;
using GiG.Core.Data.KVStores.Extensions;
using GiG.Core.Data.KVStores.Providers.Etcd.Abstractions;
using GiG.Core.Data.KVStores.Providers.Etcd.Extensions;
using GiG.Core.Data.KVStores.Providers.Hosting;
using GiG.Core.Data.KVStores.Providers.Tests.Integration.Mocks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Data.KVStores.Providers.Tests.Integration.Tests
{
    [Trait("Category", "IntegrationWithDependency")]
    public class EtcdProviderTests
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
                    var configuration = hostContext.Configuration;

                    services.AddKVStores<IEnumerable<MockLanguage>>()
                        .FromJsonEtcd(configuration, "Languages");
                })
                .Build();

            _serviceProvider = _host.Services;

            var configuration = _serviceProvider.GetRequiredService<IConfiguration>();

            var configurationSection = configuration.GetSection("Languages");

            _etcdProviderOptions = configurationSection.Get<EtcdProviderOptions>();

            _etcdClient = new EtcdClient(_etcdProviderOptions.ConnectionString);
        }

        [Fact]
        public async Task GetData_JsonEtcdProvider_ReturnsMockLanguages()
        {
            // Arrange.
            await ClearEtcdKey(_etcdProviderOptions.Key);
            await WriteToEtcd("languages.json", _etcdProviderOptions.Key);

            var languages = await ReadFromEtcd(_etcdProviderOptions.Key);

            var providerHostedService = _serviceProvider.GetRequiredService<IHostedService>() as ProviderHostedService<IEnumerable<MockLanguage>>;

            await providerHostedService?.StartAsync(CancellationToken.None);

            var dataRetriever = _serviceProvider.GetRequiredService<IDataRetriever<IEnumerable<MockLanguage>>>();

            // Act.
            IEnumerable<MockLanguage> data = dataRetriever.Get();

            // Assert.
            Assert.NotNull(data);
            Assert.Equal(languages.Select(l => l.Alpha2Code), data.Select(l => l.Alpha2Code));
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