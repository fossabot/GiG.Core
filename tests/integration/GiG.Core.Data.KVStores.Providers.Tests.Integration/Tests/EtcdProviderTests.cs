using dotnet_etcd;
using GiG.Core.Data.KVStores.Abstractions;
using GiG.Core.Data.KVStores.Extensions;
using GiG.Core.Data.KVStores.Providers.Etcd.Abstractions;
using GiG.Core.Data.KVStores.Providers.Etcd.Extensions;
using GiG.Core.Data.KVStores.Providers.Tests.Integration.Mocks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Polly.Registry;
using Polly.Retry;
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
    public class EtcdProviderTests
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly EtcdClient _etcdClient;
        private readonly AsyncRetryPolicy<bool> _polly;
        private const string PolicyRegistryName = "ETCDRegistry";

        public EtcdProviderTests()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettingsEtcd.json");
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddKVStores<IEnumerable<MockLanguage>>()
                        .FromEtcd(hostContext.Configuration, "Languages")
                        .WithJsonSerialization();
                    
                    services.AddKVStores<IEnumerable<MockCurrency>>()
                        .FromEtcd(hostContext.Configuration, "Currencies")
                        .WithJsonSerialization();
                    
                    services.AddSingleton<IRetryPolicy<bool>>(GetPolicy());
                })
                .Build();

            _serviceProvider = host.Services;

            var configuration = _serviceProvider.GetRequiredService<IConfiguration>();

            var configurationSection = configuration.GetSection("Languages");

            var etcdProviderOptions = configurationSection.Get<EtcdProviderOptions>();

            _etcdClient = new EtcdClient(etcdProviderOptions.ConnectionString);
            
            _polly = (AsyncRetryPolicy<bool>) _serviceProvider.GetService<IRetryPolicy<bool>>();

            host.Start();
        }

        [Fact]
        public async Task GetEtcdProviderData_Languages_ReturnsMockLanguages()
        {
            // Arrange.
            await Task.Delay(1000);
            var key = "languages";
            await _etcdClient.DeleteAsync(key);
            await WriteToEtcdAsync("languages.json", key);
            var languages = await ReadLanguageFromEtcdAsync(key);

            var dataRetriever = _serviceProvider.GetRequiredService<IDataRetriever<IEnumerable<MockLanguage>>>();
            
            // Act.
            IEnumerable<MockLanguage> actualData = dataRetriever.Get();

            // Assert.
            Assert.NotNull(actualData);
            Assert.Equal(languages.Select(l => l.Alpha2Code), actualData.Select(l => l.Alpha2Code));
        }   
        
        [Fact]
        public async Task PutEtcdProviderData_Currencies_ReturnsMockCurrencies()
        {
            // Arrange.
            await Task.Delay(1000);
            var key = "currencies";
            await _polly.ExecuteAsync( () =>
            {
                _etcdClient.DeleteAsync(key).GetAwaiter().GetResult();
                string aa = _etcdClient.GetValAsync(key).GetAwaiter().GetResult();
                return Task.FromResult(string.IsNullOrEmpty(aa));
            });
            
            await WriteToEtcdAsync("currencies.json", key);
            var currencies = await ReadCurrencyFromEtcdAsync(key);

            var dataRetriever = _serviceProvider.GetRequiredService<IDataRetriever<IEnumerable<MockCurrency>>>();
            var expectedData = currencies.ToList();
            expectedData.First().Code = Guid.NewGuid().ToString();
            
            // Act.
            _etcdClient.Put(key, JsonSerializer.Serialize(expectedData));
            
            // Arrange.
            await _polly.ExecuteAsync( () =>
            {
                IEnumerable<MockCurrency> actualData = dataRetriever.Get();
                Assert.Equal(actualData.Select(l => l.Code), expectedData.Select(l => l.Code));
                return Task.FromResult(true);
            });
        }

        private async Task WriteToEtcdAsync(string filePath, string key)
        {
            var fileInfo = new FileInfo(filePath);

            if (!fileInfo.Exists)
                throw new InvalidOperationException($"File '{fileInfo.FullName}' does not exist");

            await _etcdClient.PutAsync(key, fileInfo.OpenText().ReadToEnd());
        }

        private async Task<IEnumerable<MockLanguage>> ReadLanguageFromEtcdAsync(string key)
        {
            var value = await _etcdClient.GetValAsync(key);

            return string.IsNullOrEmpty(value)
                ? new List<MockLanguage>()
                : JsonSerializer.Deserialize<IEnumerable<MockLanguage>>(value);
        }

        private async Task<IEnumerable<MockCurrency>> ReadCurrencyFromEtcdAsync(string key)
        {
            var value = await _etcdClient.GetValAsync(key);

            return string.IsNullOrEmpty(value)
                ? new List<MockCurrency>()
                : JsonSerializer.Deserialize<IEnumerable<MockCurrency>>(value);
        }
        
        private static AsyncRetryPolicy<bool> GetPolicy()
        {
            var registry =  new PolicyRegistry
            {
                {
                    PolicyRegistryName, Policy.HandleResult<bool>(x => !x)
                        .WaitAndRetryAsync(20, retryAttempt => TimeSpan.FromMilliseconds(5000))
                }
            };

            return registry.Get<AsyncRetryPolicy<bool>>(PolicyRegistryName);
        }
    }
}