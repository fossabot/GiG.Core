using Bogus;
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
using System.Collections.Immutable;
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
                .ConfigureAppConfiguration((hostingContext, config) => { config.AddJsonFile("appsettingsEtcd.json"); })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddKVStores<IEnumerable<MockLanguage>>(x =>
                        x.FromEtcd(hostContext.Configuration, "Languages")
                            .AddJson());

                    services.AddKVStores<IEnumerable<MockCurrency>>(x =>
                        x.FromEtcd(hostContext.Configuration, "Currencies")
                            .AddJson());

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
            // Arrange
            const string key = "languages";
            await _etcdClient.DeleteAsync(key);
            await WriteToEtcdAsync("languages.json", key);
            var languages = await ReadLanguageFromEtcdAsync(key);

            var dataRetriever = _serviceProvider.GetRequiredService<IDataRetriever<IEnumerable<MockLanguage>>>();

            // Act
            var actualData = await dataRetriever.GetAsync();

            // Assert
            Assert.NotNull(actualData);
            Assert.Equal(languages.Select(l => l.Alpha2Code), actualData.Select(l => l.Alpha2Code));
        }

        [Fact]
        public async Task PutEtcdProviderData_Currencies_ReturnsMockCurrencies()
        {
            // Arrange
            const string key = "currencies";
            await _polly.ExecuteAsync(async () =>
            {
                await _etcdClient.DeleteAsync(key);
                var value = await _etcdClient.GetValAsync(key);

                return string.IsNullOrEmpty(value);
            });

            await WriteToEtcdAsync("currencies.json", key);
            var currencies = await ReadCurrencyFromEtcdAsync(key);

            var dataRetriever = _serviceProvider.GetRequiredService<IDataRetriever<IEnumerable<MockCurrency>>>();
            var expectedData = currencies.ToList();
            expectedData.First().Code = Guid.NewGuid().ToString();

            // Act
            await _etcdClient.PutAsync(key, JsonSerializer.Serialize(expectedData));

            // Arrange
            await _polly.ExecuteAsync(async () =>
            {
                var actualData = await dataRetriever.GetAsync();
                Assert.Equal(actualData.Select(l => l.Code), expectedData.Select(l => l.Code));

                return true;
            });
        }

        [Fact]
        public async Task EtcdProviderDataWriteAsync_Languages_Success()
        {
            // Arrange
            const string key = "languages";
            await _etcdClient.DeleteAsync(key);

            var dataRetriever = _serviceProvider.GetRequiredService<IDataRetriever<IEnumerable<MockLanguage>>>();
            var dataWriter = _serviceProvider.GetRequiredService<IDataWriter<IEnumerable<MockLanguage>>>();

            var languages = new Faker<MockLanguage>()
                .RuleFor(x => x.Alpha2Code, new Randomizer().String(2, 'a', 'z'))
                .RuleFor(x => x.Name, new Randomizer().String(100, 'a', 'z'))
                .Generate(5)
                .AsEnumerable();

            // Act
            IEnumerable<MockLanguage> actualData = null;
            await dataWriter.LockAsync(async () =>
            {
                await dataWriter.WriteAsync(languages, key);
                actualData = await dataRetriever.GetAsync(key);
            }, key);

            // Assert
            Assert.NotNull(actualData);
            Assert.Equal(languages.Select(l => l.Alpha2Code), actualData.Select(l => l.Alpha2Code));
        }

        [Fact]
        public async Task EtcdProviderData_LanguagesUpdated_Success()
        {
            // Arrange
            const string key = "languages";
            await _etcdClient.DeleteAsync(key);

            var dataRetriever = _serviceProvider.GetRequiredService<IDataRetriever<IEnumerable<MockLanguage>>>();
            var dataWriter = _serviceProvider.GetRequiredService<IDataWriter<IEnumerable<MockLanguage>>>();

            var languages = new Faker<MockLanguage>()
                .RuleFor(x => x.Alpha2Code, new Randomizer().String(2, 'a', 'z'))
                .RuleFor(x => x.Name, new Randomizer().String(100, 'a', 'z'))
                .Generate(5)
                .AsEnumerable();

            var languagesUpdated = new List<MockLanguage>(languages);
            languagesUpdated.ToImmutableArray().AddRange(new Faker<MockLanguage>()
                .RuleFor(x => x.Alpha2Code, new Randomizer().String(2, 'a', 'z'))
                .RuleFor(x => x.Name, new Randomizer().String(100, 'a', 'z'))
                .Generate(2));

            // Act
            IEnumerable<MockLanguage> actualData = null;
            await dataWriter.LockAsync(async () =>
            {
                await dataWriter.WriteAsync(languages, key);
                actualData = await dataRetriever.GetAsync(key);
            }, key);

            IEnumerable<MockLanguage> actualDataUpdated = null;
            await dataWriter.LockAsync(async () =>
            {
                actualData = await dataRetriever.GetAsync(key);
                await dataWriter.WriteAsync(languagesUpdated, key);
                actualDataUpdated = await dataRetriever.GetAsync(key);
            }, key);

            // Assert
            Assert.NotNull(actualData);
            Assert.Equal(languages.Select(l => l.Alpha2Code), actualData.Select(l => l.Alpha2Code));
            Assert.NotNull(actualDataUpdated);
            Assert.Equal(languagesUpdated.Select(l => l.Alpha2Code), actualDataUpdated.Select(l => l.Alpha2Code));
        }

        private async Task WriteToEtcdAsync(string filePath, string key)
        {
            var fileInfo = new FileInfo(filePath);

            if (!fileInfo.Exists)
            {
                throw new InvalidOperationException($"File '{fileInfo.FullName}' does not exist");
            }

            await _etcdClient.PutAsync(key, await fileInfo.OpenText().ReadToEndAsync());
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
            var registry = new PolicyRegistry
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