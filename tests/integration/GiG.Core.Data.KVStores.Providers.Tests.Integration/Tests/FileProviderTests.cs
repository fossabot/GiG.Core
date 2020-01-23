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
using Xunit;

namespace GiG.Core.Data.KVStores.Providers.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    public class FileProviderTests
    {
        private readonly IHost _host;
        private readonly IServiceProvider _serviceProvider;

        public FileProviderTests()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettingsFile.json");
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
        public void GetData_JsonFileProvider_ReturnsMockLanguages()
        {
            // Arrange.
            var languages = ReadFromJson(_serviceProvider);
            
            var dataRetriever = _serviceProvider.GetRequiredService<IDataRetriever<IEnumerable<MockLanguage>>>();
            
            // Act.
            var data = dataRetriever.Get();
            
            // Assert.
            Assert.NotNull(data);
            Assert.Equal(languages.Select(l => l.Alpha2Code), data.Select(l => l.Alpha2Code));
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
    }
}