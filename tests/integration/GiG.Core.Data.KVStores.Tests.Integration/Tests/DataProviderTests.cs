using GiG.Core.Data.KVStores.Abstractions;
using GiG.Core.Data.KVStores.Providers.FileProviders.Abstractions;
using GiG.Core.Data.KVStores.Providers.Hosting;
using GiG.Core.Data.KVStores.Tests.Integration.Mocks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using Xunit;

namespace GiG.Core.Data.KVStores.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    public class DataProviderTests
    {
        [Fact]
        public void GetData_JsonDataProvider_ReturnsMockLanguages()
        {
            // Arrange.
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices(MockStartup.ConfigureServices)
                .Build();

            var serviceProvider = host.Services;
            var languages = ReadFromJson(serviceProvider);
            
            var providerHostedService = serviceProvider.GetRequiredService<IHostedService>() as ProviderHostedService<IEnumerable<MockLanguage>>;
            
             providerHostedService?.StartAsync(CancellationToken.None);
            
            var dataRetriever = serviceProvider.GetRequiredService<IDataRetriever<IEnumerable<MockLanguage>>>();
            
            // Act.
            var data = dataRetriever.Get();
            
            //Assert.
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